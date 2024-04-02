using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Order
{
    public class DisplayOrderFiltersDTO
    {
        public string searchKeyword { get; set; } = string.Empty;
        public int? technicianId { get; set; }
        public bool? isPaid { get; set; }
        public bool? isLocked { get; set; }
        public bool? isArchived { get; set; }
        public bool? hasWarranty { get; set; }
        public int? statusId { get; set; }
        public int? priorityId { get; set; }
        public string sortBy { get; set; } = string.Empty;
        public bool isSortAscending { get; set; }
        public int pageSize { get; set; }
    }
}
