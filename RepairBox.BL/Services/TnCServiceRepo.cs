using RepairBox.BL.DTOs.TermsAndConditions;
using RepairBox.BL.ServiceModels.Brand;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface ITnCServiceRepo
    {
        List<GetTnCDTO> GetTnC();
        GetTnCDTO? GetTnC(int Id);
        void AddTnC(AddTnCDTO dto);
        void AddTnC(List<AddTnCDTO> dtos);
        bool UpdateTnC(UpdateTnCDTO dto);
        bool DeleteTnC(int Id);
    }
    public class TnCServiceRepo : ITnCServiceRepo
    {
        public ApplicationDBContext _context;
        public TnCServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public List<GetTnCDTO> GetTnC()
        {
            var tncs = _context.TermsAndConditions.ToList();

            var tncList = new List<GetTnCDTO>();

            if(tncs != null)
            {
                foreach (var tnc in tncs)
                {
                    tncList.Add(new GetTnCDTO
                    {
                        Id = tnc.Id,
                        Title = tnc.Title,
                        Content = tnc.Content,
                        UpdatedAt = tnc.UpdatedAt,
                        CreatedAt = tnc.CreatedAt
                    });
                }
            }

            return tncList;
        }
        public GetTnCDTO? GetTnC(int Id)
        {
            var tnc = _context.TermsAndConditions.FirstOrDefault(tnc => tnc.Id == Id);
            
            if (tnc == null) { return null; }

            return Omu.ValueInjecter.Mapper.Map<GetTnCDTO>(tnc);
        }
        public void AddTnC(AddTnCDTO dto)
        {
            _context.TermsAndConditions.Add(new TermsAndConditions
            {
                Title = dto.Title,
                Content = dto.Content,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            });

            _context.SaveChanges();
        }
        public void AddTnC(List<AddTnCDTO> dtos)
        {
            foreach(var dto in dtos)
            {
                _context.TermsAndConditions.Add(new TermsAndConditions
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                });
            }

            _context.SaveChanges();
        }
        public bool UpdateTnC(UpdateTnCDTO dto)
        {
            var tnc = _context.TermsAndConditions.FirstOrDefault(tnc => tnc.Id == dto.Id);
            
            if (tnc == null) { return false; }

            tnc.Title = dto.Title;
            tnc.Content = dto.Content;
            tnc.UpdatedAt = DateTime.Now;
            
            _context.SaveChanges();

            return true;
        }
        public bool DeleteTnC(int Id)
        {
            var tnc = _context.TermsAndConditions.FirstOrDefault(tnc => tnc.Id == Id);

            if (tnc == null) { return false; }

            _context.TermsAndConditions.Remove(tnc);
            _context.SaveChanges();

            return true;
        }
    }
}
