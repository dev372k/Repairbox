using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class CustomerInfo : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<CustomerIdentities> CustomerIdentities { get; set; }
        public ICollection<DeviceInfo> DeviceInfos { get; set; }
        public ICollection<PurchaseFromCustomerInvoice> PurchaseFromCustomerInvoices { get; set; }
    }
}
