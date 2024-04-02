using RepairBox.BL.DTOs.PrivacyPolicy;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface IPrivacyPolicyServiceRepo
    {
        List<GetPrivacyPolicyDTO> GetPrivacyPolicy();
        GetPrivacyPolicyDTO? GetPrivacyPolicy(int Id);
        void AddPrivacyPolicy(AddPrivacyPolicyDTO dto);
        void AddPrivacyPolicy(List<AddPrivacyPolicyDTO> dtos);
        bool UpdatePrivacyPolicy(UpdatePrivacyPolicyDTO dto);
        bool DeletePrivacyPolicy(int Id);
    }

    public class PrivacyPolicyServiceRepo : IPrivacyPolicyServiceRepo
    {
        public ApplicationDBContext _context;
        public PrivacyPolicyServiceRepo(ApplicationDBContext context) 
        {
            _context = context;
        }
        public List<GetPrivacyPolicyDTO> GetPrivacyPolicy()
        {
            var privacyPolicies = _context.PrivacyPolicies.ToList();

            var privacyPolicyList = new List<GetPrivacyPolicyDTO>();

            if (privacyPolicies != null)
            {
                foreach (var privacyPolicy in privacyPolicies)
                {
                    privacyPolicyList.Add(new GetPrivacyPolicyDTO
                    {
                        Id = privacyPolicy.Id,
                        Title = privacyPolicy.Title,
                        Content = privacyPolicy.Content,
                        UpdatedAt = privacyPolicy.UpdatedAt,
                        CreatedAt = privacyPolicy.CreatedAt
                    });
                }
            }

            return privacyPolicyList;
        }
        public GetPrivacyPolicyDTO? GetPrivacyPolicy(int Id)
        {
            var privacyPolicy = _context.PrivacyPolicies.FirstOrDefault(p => p.Id == Id);

            if (privacyPolicy == null) { return null; }

            return Omu.ValueInjecter.Mapper.Map<GetPrivacyPolicyDTO>(privacyPolicy);
        }
        public void AddPrivacyPolicy(AddPrivacyPolicyDTO dto)
        {
            _context.PrivacyPolicies.Add(new PrivacyPolicy
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
        public void AddPrivacyPolicy(List<AddPrivacyPolicyDTO> dtos)
        {
            foreach (var dto in dtos)
            {
                _context.PrivacyPolicies.Add(new PrivacyPolicy
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
        public bool UpdatePrivacyPolicy(UpdatePrivacyPolicyDTO dto)
        {
            var privacyPolicy = _context.PrivacyPolicies.FirstOrDefault(p => p.Id == dto.Id);

            if (privacyPolicy == null) { return false; }

            privacyPolicy.Title = dto.Title;
            privacyPolicy.Content = dto.Content;
            privacyPolicy.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return true;
        }
        public bool DeletePrivacyPolicy(int Id)
        {
            var privacyPolicy = _context.PrivacyPolicies.FirstOrDefault(p => p.Id == Id);

            if (privacyPolicy == null) { return false; }

            _context.PrivacyPolicies.Remove(privacyPolicy);
            _context.SaveChanges();

            return true;
        }
    }
}
