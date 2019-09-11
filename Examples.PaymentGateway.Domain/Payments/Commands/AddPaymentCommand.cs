using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public class AddPaymentCommand
    {
        /// <summary>
        /// The 3 letter ISO currency code of the payment. Cannot be 
        /// null and must be uppercase.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The payment amount in the specified currency.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The details of the credit card to take payment from.
        /// </summary>
        public CreditCard CreditCard { get; set; }
    }
}
