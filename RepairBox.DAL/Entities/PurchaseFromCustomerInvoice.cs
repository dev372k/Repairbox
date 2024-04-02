using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class PurchaseFromCustomerInvoice : Base
    {
        public long invoiceId { get; set; }
        [ForeignKey("CustomerInfo")]
        public int CustomerInfoId { get; set; }
        public DateTime Date { get; set; }
        public string QRCodePath { get; set; } = string.Empty;
    }
}
