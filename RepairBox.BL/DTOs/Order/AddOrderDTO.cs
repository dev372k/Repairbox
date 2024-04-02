using RepairBox.Common.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.ServiceModels.Order
{
    public class AddOrderDTO
    {
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public List<int> RepairableDefects { get; set; }
        public int PriorityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Diagnostics { get; set; } = string.Empty;
        public bool WarrantyStatus { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string CardType { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public long Amount { get; set; }
    }
}
