using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain.Internal
{
    /// <summary>
    /// An abstraction over the payments data store, which is
    /// outside the scope of this example.
    /// </summary>
    public interface IPaymentRepository
    {
        Task<int> StartPaymentAsync(int merchantId, AddPaymentCommand command);

        Task CompletePaymentAsync(int paymentId, BankPaymentResponse bankPaymentResponse);

        Task<PaymentDetails> GetPaymentDetailsByPaymentIdAsync(int paymentId);
    }
}
