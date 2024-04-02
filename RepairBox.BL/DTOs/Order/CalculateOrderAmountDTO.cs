using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Order
{
    public class CalculateOrderAmountDTO
    {
        public decimal SubTotal { get; set; }
        public decimal PriorityProcessCharges { get; set; }
        public decimal Tax { get; set; }
        public decimal Profit { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
