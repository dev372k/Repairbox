using RepairBox.BL.DTOs.Priority;
using RepairBox.BL.DTOs.Status;
using RepairBox.Common.Commons;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface IPriorityServiceRepo
    {
        List<GetPriorityDTO>? GetPriorities();
        List<GetPriorityDropdownDTO>? GetPrioritiesForDropdown();
        GetPriorityDTO? GetPriorityById(int id);
        void AddPriority(AddPriorityDTO priorityDTO);
        void UpdatePriority(UpdatePriorityDTO priorityDTO);
        bool DeletePriority(int id);
    }

    public class PriorityServiceRepo : IPriorityServiceRepo
    {
        public ApplicationDBContext _context;
        private int pageSize = DeveloperConstants.PAGE_SIZE;
        public PriorityServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<GetPriorityDTO>? GetPriorities()
        {
            List<GetPriorityDTO> priorities = new List<GetPriorityDTO>();
            var priorityList = _context.RepairPriorities.ToList();
            if (priorityList != null)
            {
                priorityList.ForEach(priority => priorities.Add(Omu.ValueInjecter.Mapper.Map<GetPriorityDTO>(priority)));
                return priorities;
            }
            return null;
        }

        public List<GetPriorityDropdownDTO>? GetPrioritiesForDropdown()
        {
            var priorities = new List<GetPriorityDropdownDTO>();
            var priorityList = _context.RepairPriorities.ToList();
            if (priorityList != null)
            {
                priorityList.ForEach(priority => priorities.Add(Omu.ValueInjecter.Mapper.Map<GetPriorityDropdownDTO>(priority)));
                return priorities;
            }
            return null;
        }

        public GetPriorityDTO? GetPriorityById(int id)
        {
            var priority = _context.RepairPriorities.FirstOrDefault(p => p.Id == id);
            if (priority != null)
            {
                var dbRequest = Omu.ValueInjecter.Mapper.Map<GetPriorityDTO>(priority);
                return dbRequest;
            }
            return null;
        }

        public void AddPriority(AddPriorityDTO priorityDTO)
        {
            var priority = new RepairPriority
            {
                Name = priorityDTO.Name,
                ProcessCharges = priorityDTO.ProcessCharges
            };

            _context.RepairPriorities.Add(priority);
            _context.SaveChanges();

            return ;
        }

        public void UpdatePriority(UpdatePriorityDTO priorityDTO)
        {
            var priority = _context.RepairPriorities.FirstOrDefault(p => p.Id == priorityDTO.Id);
            if(priority != null)
            {
                priority.Name = priorityDTO.Name;
                priority.ProcessCharges = priorityDTO.ProcessCharges;

                _context.SaveChanges();
            }

            return ;
        }

        public bool DeletePriority(int id)
        {
            var priority = _context.RepairPriorities.FirstOrDefault(p => p.Id == id);
            if(priority != null)
            {
                _context.RepairPriorities.Remove(priority);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
