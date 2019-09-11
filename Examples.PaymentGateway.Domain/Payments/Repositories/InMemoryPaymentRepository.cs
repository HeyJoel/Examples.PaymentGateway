using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain.Internal
{
    public class InMemoryPaymentsRepository : IPaymentRepository
    {
        private readonly Dictionary<int, PaymentDetails> _payments = new Dictionary<int, PaymentDetails>();
        private readonly ILogger<InMemoryPaymentsRepository> _logger;
        private int _currentId = 0;

        public InMemoryPaymentsRepository(
            ILogger<InMemoryPaymentsRepository> logger
            )
        {
            _logger = logger;
        }

        public Task<int> StartPaymentAsync(int merchantId, AddPaymentCommand command)
        {
            // threadsafe id increment because the repository is singleton scope
            var paymentId = Interlocked.Increment(ref _currentId);

            var payment = new PaymentDetails()
            {
                PaymentId = paymentId,
                Amount = command.Amount,
                Currency = command.Currency,
                MaskedCreditCardNumber = CreditCardNumberFormatter.Mask(command.CreditCard.CardNumber),
                MerchantId = merchantId,
                RequestedDate = DateTime.UtcNow,
                Status = PaymentStatus.Started
            };

            _payments.Add(paymentId, payment);

            _logger.LogDebug("Started payment successfully for merchant {MerchantId}. PaymentId: {PaymentId}", merchantId, paymentId);

            return Task.FromResult(paymentId);
        }

        public Task CompletePaymentAsync(int paymentId, BankPaymentResponse bankPaymentResponse)
        {
            if (!_payments.TryGetValue(paymentId, out PaymentDetails result))
            {
                throw new Exception($"Cannot complete payment {paymentId}, payment record not found.");
            }

            result.BankPaymentId = bankPaymentResponse.BankPaymentId;
            result.Status = bankPaymentResponse.Result;
            result.ResponseReceivedDate = DateTime.UtcNow;

            _logger.LogDebug("Completed payment for paymentId {PaymentId} with a status of {Status}", paymentId, bankPaymentResponse.Result);

            return Task.CompletedTask;
        }

        public Task<PaymentDetails> GetPaymentDetailsByPaymentIdAsync(int paymentId)
        {
            _payments.TryGetValue(paymentId, out PaymentDetails result);

            var found = result != null;
            _logger.LogDebug("Retreived payment record {PaymentId}, result: {IsFound}", paymentId, found);

            return Task.FromResult(result);
        }
    }
}
