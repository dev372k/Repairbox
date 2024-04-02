using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Dashboard
{
    public class GetDashboardDTO
    {
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public decimal Tax { get; set; }
        public decimal Profit { get; set; }
        public int RepairOrders { get; set; }
        public int Brands { get; set; }
        public int Devices { get; set; }
        public int Defects { get; set; }
        public List<AnnualRepairOrder> AnnualRepairOrders { get; set; }
    }

    public class AnnualRepairOrder
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int OrderCount { get; set; }
    }
}
