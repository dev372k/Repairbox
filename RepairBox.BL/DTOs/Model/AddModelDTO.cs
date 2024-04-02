using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Model
{
    public class AddModelDTO
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public int BrandId { get; set; }
    }
}
