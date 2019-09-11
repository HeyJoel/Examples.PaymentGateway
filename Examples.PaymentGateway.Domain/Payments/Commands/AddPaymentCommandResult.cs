using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public class AddPaymentCommandResult
    {
        public int PaymentId { get; set; }

        public PaymentStatus PaymentResult { get; set; }
    }
}
