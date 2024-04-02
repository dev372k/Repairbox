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
    public interface IStatusServiceRepo
    {
        List<GetStatusDTO>? GetStatuses();
        GetStatusDTO? GetStatusById(int id);
        void AddStatus(AddStatusDTO statusDTO);
        void UpdateStatus(UpdateStatusDTO statusDTO);
        bool DeleteStatus(int id);
    }

    public class StatusServiceRepo : IStatusServiceRepo
    {
        public ApplicationDBContext _context;
        private int pageSize = DeveloperConstants.PAGE_SIZE;
        public StatusServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<GetStatusDTO>? GetStatuses()
        {
            List<GetStatusDTO> statuses = new List<GetStatusDTO>();
            var statusList = _context.RepairStatuses.ToList();
            if (statusList != null)
            {
                statusList.ForEach(status => statuses.Add(Omu.ValueInjecter.Mapper.Map<GetStatusDTO>(status)));
                return statuses;
            }
            return null;
        }

        public GetStatusDTO? GetStatusById(int id)
        {
            var status = _context.RepairStatuses.FirstOrDefault(p => p.Id == id);
            if (status != null)
            {
                var dbRequest = Omu.ValueInjecter.Mapper.Map<GetStatusDTO>(status);
                return dbRequest;
            }
            return null;
        }

        public void AddStatus(AddStatusDTO statusDTO)
        {
            var status = new RepairStatus
            {
                Name = statusDTO.Name
            };

            _context.RepairStatuses.Add(status);
            _context.SaveChanges();

            return ;
        }

        public void UpdateStatus(UpdateStatusDTO statusDTO)
        {
            var status = _context.RepairStatuses.FirstOrDefault(p => p.Id == statusDTO.Id);
            if(status != null)
            {
                status.Name = statusDTO.Name;

                _context.SaveChanges();
            }

            return ;
        }

        public bool DeleteStatus(int id)
        {
            var status = _context.RepairStatuses.FirstOrDefault(s => s.Id == id);
            if(status != null)
            {
                _context.RepairStatuses.Remove(status);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
