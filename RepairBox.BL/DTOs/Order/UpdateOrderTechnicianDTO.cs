using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Order
{
    public class UpdateOrderTechnicianDTO
    {
        public int OrderId { get; set; }
        public int TechnicianId { get; set; }
    }
}
