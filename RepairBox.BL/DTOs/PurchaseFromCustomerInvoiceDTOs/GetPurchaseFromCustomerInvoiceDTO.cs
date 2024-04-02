using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.PurchaseFromCustomerInvoiceDTOs
{
    public class GetPurchaseFromCustomerInvoiceDTO
    {
        public long invoiceId { get; set; }
        public DateTime Date { get; set; }
        public string QRCodePath { get; set; }
    }
}
