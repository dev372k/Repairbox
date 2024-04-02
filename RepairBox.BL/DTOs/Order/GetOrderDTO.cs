using RepairBox.BL.DTOs.RepairDefect;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Order
{
    public class GetOrderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Diagnostics { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int? TechnicianId { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
        public int PriorityId { get; set; }
        public string PriorityName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public bool isPaid { get; set; }
        public decimal AmountPaid { get; set; }
        public bool WarrantyStatus { get; set; }
        public CalculateOrderAmountDTO? OrderAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GetDefectForOrderDTO>? Defects { get; set; }
    }
}
