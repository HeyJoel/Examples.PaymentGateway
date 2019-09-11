using Examples.PaymentGateway.Domain.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain
{
    public class GetPaymentDetailsByPaymentIdQueryHandler
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentDetailsByPaymentIdQueryHandler(
            IUserSessionService userSessionService,
            IPaymentRepository paymentRepository
            )
        {
            _userSessionService = userSessionService;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDetails> ExecuteAsync(GetPaymentDetailsByPaymentIdQuery query)
        {
            var paymentDetails = await _paymentRepository.GetPaymentDetailsByPaymentIdAsync(query.PaymentId);
            var merchantId = _userSessionService.GetCurrentMerchantId();

            if (paymentDetails != null && paymentDetails.MerchantId != merchantId)
            {
                // Depending on requirements you could choose to return null here
                // instead and log a warning or similar.
                // E.g throwing an exception could allow an attacker to validate paymentIds
                // but that may be considered irrelevant if a valid paymentId is of no use 
                // to an attacker.
                var msg = $"Merchant {merchantId} is not permitted to view payments for merchant {paymentDetails.MerchantId}.";
                throw new NotPermittedException(msg);
            }

            return paymentDetails;
        }
    }
}
