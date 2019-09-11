using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public class CreditCard
    {
        public string CardNumber { get; set; }

        public string NameOnCard { get; set; }

        public string CCV { get; set; }

        public int ExpiryMonth { get; set; }

        /// <summary>
        /// 4 digit year of expiry.
        /// </summary>
        public int ExpiryYear { get; set; }
    }
}
