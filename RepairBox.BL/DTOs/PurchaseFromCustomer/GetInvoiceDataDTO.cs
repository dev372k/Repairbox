using RepairBox.BL.DTOs.CustomerIdentities;
using RepairBox.BL.DTOs.CustomerInfo;
using RepairBox.BL.DTOs.DeviceInfo;
using RepairBox.BL.DTOs.PurchaseFromCustomerInvoiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.CustomerProductPurchase
{
    public class GetInvoiceDataDTO
    {
        public GetCustomerInfoDTO CustomerInfo { get; set; }
        public GetDeviceInfoDTO DeviceInfo { get; set; }
        public GetPurchaseFromCustomerInvoiceDTO PurchaseFromCustomerInvoice { get; set; }
    }
}
