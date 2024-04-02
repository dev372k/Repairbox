using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Dashboard
{
    public class DisplayDashboardFiltersDTO
    {
        public bool? isPaid { get; set; }
        public int? statusId { get; set; }
        public int? priorityId { get; set; }
        public int? technicianId { get; set; }
    }
}
