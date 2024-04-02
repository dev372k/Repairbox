using RepairBox.Common.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class Order : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Diagnostics { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Profit { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public bool IsPaid { get; set; }           
        public bool WarrantyStatus { get; set; }
        [ForeignKey("Model")]
        public int ModelId { get; set; }
        public int? TechnicianId { get; set; }
        public int PriorityId { get; set; }        
        public int StatusId { get; set; }
        public ICollection<OrderDefect> OrderDefects { get; set; }
        public Order()
        {
            if (PaymentMethod == enPaymentMethod.CoD)
                IsPaid = false;
        }
    }
}
