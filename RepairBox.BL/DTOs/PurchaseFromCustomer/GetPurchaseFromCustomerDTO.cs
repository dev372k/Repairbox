using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.PurchaseFromCustomer
{
    public class GetPurchaseFromCustomerDTO
    {
        public int Id { get; set; }
        public long InvoiceId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string DeviceNameModel { get; set; } = string.Empty;
        public string DeviceIMEI { get; set; } = string.Empty;
        public string DeviceSerialNumber { get; set; } = string.Empty;
        public decimal DeviceCost { get; set; }
        public decimal DevicePrice { get; set; }
        public DateTime Date { get; set; }
        public string QRCodePath { get; set; } = string.Empty;
    }
}
