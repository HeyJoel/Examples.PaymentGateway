using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public class AddBankPaymentCommand
    {
        /// <summary>
        /// Assumption: the bank payment service knows how
        /// to pay the merchants account.
        /// </summary>
        public int MerchantId { get; set; }

        public int PaymentId { get; set; }

        public CreditCard CreditCard { get; set; }
    }
}
