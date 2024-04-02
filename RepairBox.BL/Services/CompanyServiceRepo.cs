using Microsoft.EntityFrameworkCore;
using RepairBox.BL.DTOs.Company;
using RepairBox.BL.DTOs.CustomerInfo;
using RepairBox.BL.DTOs.CustomerProductPurchase;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface ICompanyServiceRepo
    {
        public GetCompanyDTO? GetCompany();
    }
    public class CompanyServiceRepo : ICompanyServiceRepo
    {
        public ApplicationDBContext _context;
        public CompanyServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public GetCompanyDTO? GetCompany()
        {
            var company = _context.Companies.FirstOrDefault();

            if(company == null) { return null; }

            return Omu.ValueInjecter.Mapper.Map<GetCompanyDTO>(company);
        }
    }
}
