using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain.Internal
{
    public class MockAquiringBankService : IAquiringBankService
    {
        private int _currentId = 0;

        public virtual Task<BankPaymentResponse> MakePaymentAsync(AddBankPaymentCommand command)
        {
            // threadsafe id increment because the repository is singleton scope
            var bankPaymentId = Interlocked.Increment(ref _currentId);

            var response = new BankPaymentResponse()
            {
                BankPaymentId = bankPaymentId.ToString(),
                Result = PaymentStatus.Paid
            };

            return Task.FromResult(response);
        }
    }
}
