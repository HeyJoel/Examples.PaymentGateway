using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public class PaymentDetails
    {
        /// <summary>
        /// Database identifier for the payment record.
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// The identifier of the merchant that initiated the
        /// payment request and is to be paid.
        /// </summary>
        public int MerchantId { get; set; }

        /// <summary>
        /// The identifier returned by the bank in the payment
        /// request. Mat be null if the request has not been
        /// sent yet.
        /// </summary>
        public string BankPaymentId { get; set; }

        /// <summary>
        /// 3 letter ISO currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The amount requested to be paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The credit card number with only the last 4 numbers exposed.
        /// </summary>
        public string MaskedCreditCardNumber { get; set; }

        /// <summary>
        /// The current status of the payment.
        /// </summary>
        public PaymentStatus Status { get; set; }

        /// <summary>
        /// The date the payment request was received (UTC).
        /// </summary>
        public DateTime RequestedDate { get; set; }

        /// <summary>
        /// The date the response from the aquiring bank was received (UTC).
        /// </summary>
        public DateTime? ResponseReceivedDate { get; set; }
    }
}
