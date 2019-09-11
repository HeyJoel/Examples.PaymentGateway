using Examples.PaymentGateway.Domain.Internal;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Examples.PaymentGateway.Domain.Tests
{
    public class AddPaymentCommandHandlerTests
    {
        [Fact]
        public Task Execute_WhenCommandInvalid_ThrowsException()
        {
            var handler = new AddPaymentCommandHandler(
                NullLogger<AddPaymentCommandHandler>.Instance,
                new MockAquiringBankService(),
                new MockUserSessionService(),
                new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance)
                );

            var command = AddPaymentCommandValidatorTests.CreateValidCommand();
            command.Currency = null;

            return Assert.ThrowsAsync<ValidationException>(() => handler.ExecuteAsync(command));
        }

        [Fact]
        public async Task Execute_WhenCommandValid_PaymentSavedToRepository()
        {
            var paymentRepository = new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance);

            var handler = new AddPaymentCommandHandler(
                NullLogger<AddPaymentCommandHandler>.Instance,
                new MockAquiringBankService(),
                new MockUserSessionService(),
                paymentRepository
                );

            var command = AddPaymentCommandValidatorTests.CreateValidCommand();
            var result = await handler.ExecuteAsync(command);
            var paymentDetails = await paymentRepository.GetPaymentDetailsByPaymentIdAsync(result.PaymentId);

            Assert.NotNull(paymentDetails);
        }

        [Fact]
        public async Task Execute_WhenCommandValid_CallsAquiringBankService()
        {
            const int MERCHANT_ID = 4;

            AddBankPaymentCommand addBankPaymentCommand = null;
            var aquiringBankService = new Mock<MockAquiringBankService>();
            aquiringBankService
                .Setup(m => m.MakePaymentAsync(It.IsAny<AddBankPaymentCommand>()))
                .Callback<AddBankPaymentCommand>(c => addBankPaymentCommand = c)
                .CallBase();

            var userSessionService = new Mock<IUserSessionService>();
            userSessionService
                .Setup(m => m.GetCurrentMerchantId())
                .Returns(MERCHANT_ID);

            var handler = new AddPaymentCommandHandler(
                NullLogger<AddPaymentCommandHandler>.Instance,
                aquiringBankService.Object,
                userSessionService.Object,
                new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance)
                );

            var command = AddPaymentCommandValidatorTests.CreateValidCommand();

            await handler.ExecuteAsync(command);

            Assert.Equal(command.CreditCard, addBankPaymentCommand.CreditCard);
            Assert.Equal(MERCHANT_ID, addBankPaymentCommand.MerchantId);
            Assert.True(addBankPaymentCommand.PaymentId > 0);
        }
    }
}
