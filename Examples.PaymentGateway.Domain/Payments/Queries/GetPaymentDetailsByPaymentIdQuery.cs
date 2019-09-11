using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public class GetPaymentDetailsByPaymentIdQuery
    {
        public GetPaymentDetailsByPaymentIdQuery() { }

        public GetPaymentDetailsByPaymentIdQuery(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; set; }
    }
}
