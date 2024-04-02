using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.PurchaseFromCustomer
{
    public class DisplayPurchaseFromCustomerFiltersDTO
    {
        public string searchKeyword { get; set; } = string.Empty;
        public string sortBy { get; set; } = string.Empty;
        public bool isSortAscending { get; set; }
        public int pageSize { get; set; }
    }
}
