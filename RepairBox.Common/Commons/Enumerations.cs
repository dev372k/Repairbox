using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.Common.Commons
{
    public enum enPaymentMethod
    {
        CoD = 0,
        Card = 1
    }

    public enum enStripeChargeStatus
    {
        succeeded, 
        failed, 
        pending
    }
}
