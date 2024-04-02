using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Company
{
    public class GetCompanyDTO
    {
        public string Name { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
    }
}
