using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class Transaction : Base
    {
        public string Email { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string CardMask { get; set; } = string.Empty;
        public string CardType { get; set; } = string.Empty;
        public string StripeCustomerId { get; set; } = string.Empty;
        public string StripeTransactionId { get; set; } = string.Empty;
    }
}
