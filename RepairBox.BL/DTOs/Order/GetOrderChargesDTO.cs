using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Order
{
    public class GetOrderChargesDTO
    {
        public List<int> Defects { get; set; }
        public int PriorityId { get; set; }
    }
}
