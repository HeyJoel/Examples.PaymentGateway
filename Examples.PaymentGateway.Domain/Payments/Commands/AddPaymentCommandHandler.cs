using Examples.PaymentGateway.Domain.Internal;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain
{
    public class AddPaymentCommandHandler
    {
        private readonly ILogger<AddPaymentCommandHandler> _logger;
        private readonly IAquiringBankService _aquiringBankService;
        private readonly IUserSessionService _userSessionService;
        private readonly IPaymentRepository _paymentRepository;

        public AddPaymentCommandHandler(
            ILogger<AddPaymentCommandHandler> logger,
            IAquiringBankService aquiringBankService,
            IUserSessionService userSessionService,
            IPaymentRepository paymentRepository
            )
        {
            _logger = logger;
            _aquiringBankService = aquiringBankService;
            _userSessionService = userSessionService;
            _paymentRepository = paymentRepository;
        }

        public async Task<AddPaymentCommandResult> ExecuteAsync(AddPaymentCommand command)
        {
            await InitializeExecution(command);

            // Assumption: the API would have some kind of auth that would allow us
            // to know the merchant making the API call.
            var merchantId = _userSessionService.GetCurrentMerchantId();
            _logger.LogDebug("Adding payment for merchant {MerchantId}", merchantId);

            // store the payment attempt to ensure the request is captured even
            // if an exception occurs during payment e.g. the bank payment is successful
            // but updating the status in the data store fails due to a network error
            var paymentId = await _paymentRepository.StartPaymentAsync(merchantId, command);

            // Try and register the payment with the bank 
            var addBankPaymentCommand = new AddBankPaymentCommand()
            {
                CreditCard = command.CreditCard,
                MerchantId = merchantId,
                PaymentId = paymentId
            };
            var bankPaymentResponse = await _aquiringBankService.MakePaymentAsync(addBankPaymentCommand);

            // Update the payment attempt with the result
            await _paymentRepository.CompletePaymentAsync(paymentId, bankPaymentResponse);

            // return the result
            var result = new AddPaymentCommandResult()
            {
                PaymentId = paymentId,
                PaymentResult = bankPaymentResponse.Result
            };

            _logger.LogDebug("Payment completed with a result of {Result} for paymentId {PaymentId}", bankPaymentResponse.Result, paymentId);
            return result;
        }

        /// <summary>
        /// Typically I'd look to handle command execution and these sorts of
        /// cross-cutting concerns via a framework e.g. Mediator pipelines.
        /// 
        /// This could also include authentication/authorization. If a
        /// re-usable domain is not important you could also do these cross
        /// cutting concerns in the web api layer via filters etc.
        /// </summary>
        private async Task InitializeExecution(AddPaymentCommand command)
        {
            _logger.LogInformation("Executing AddPaymentCommandHandler");
            if (command == null) throw new ArgumentNullException(nameof(command));

            // In this example validation is handled in the API layer first to
            // provide user-friendly error messages in the response.
            // It's validated again here as a back-stop; I like my command handlers
            // not to assume anything, however it could be ommited depending on
            // overall approach.
            var validator = new AddPaymentCommandValidator();
            await validator.ValidateAndThrowAsync(command);
        }
    }
}
