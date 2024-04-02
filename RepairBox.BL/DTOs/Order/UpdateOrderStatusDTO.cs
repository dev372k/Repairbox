using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Order
{
    public class UpdateOrderStatusDTO
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
    }
}
