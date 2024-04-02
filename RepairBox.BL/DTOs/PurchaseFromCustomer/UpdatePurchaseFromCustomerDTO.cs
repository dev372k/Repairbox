using RepairBox.BL.DTOs.CustomerIdentities;
using RepairBox.BL.DTOs.CustomerInfo;
using RepairBox.BL.DTOs.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.CustomerProductPurchase
{
    public class UpdatePurchaseFromCustomerDTO
    {
        public UpdateCustomerInfoDTO CustomerInfo { get; set; }
        public UpdateCustomerIdentitiesDTO CustomerIdentities { get; set; }
        public UpdateDeviceInfoDTO DeviceInfo { get; set; }
        public string invoiceId { get; set; }
    }
}
