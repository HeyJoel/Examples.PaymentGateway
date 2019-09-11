using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public enum PaymentStatus
    {
        /// <summary>
        /// Default/Not started state.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The payment has been received and logged but not processed.
        /// </summary>
        Started,

        /// <summary>
        /// The payment was declined by the aquiring bank.
        /// </summary>
        Declined,

        /// <summary>
        /// The payment has been completed by the aquiring bank.
        /// </summary>
        Paid
    }
}
