using RepairBox.Common.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Stripe
{
    public class StripeChargeReponseDTO
    {
        public enStripeChargeStatus Status { get; set; }
        public string StripeCustomerId { get; set; } = string.Empty;
        public string StripeTransactionId { get; set; } = string.Empty;
    }
}
