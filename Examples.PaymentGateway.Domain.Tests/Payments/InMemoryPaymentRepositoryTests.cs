using Examples.PaymentGateway.Domain.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Examples.PaymentGateway.Domain.Tests
{
    public class InMemoryPaymentRepositoryTests
    {
        [Fact]
        public async Task StartPaymentAsync_WithValidCommmand_AddsToStore()
        {
            var command = CreateValidAddPaymentCommand();
            var repository = new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance);

            var id = await repository.StartPaymentAsync(1, command);
            var result = await repository.GetPaymentDetailsByPaymentIdAsync(id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task StartPaymentAsync_StartMultiple_IncrementsId()
        {
            var command = CreateValidAddPaymentCommand();
            var repository = new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance);

            var firstId = await repository.StartPaymentAsync(1, command);
            var secondId = await repository.StartPaymentAsync(1, command);

            Assert.True(firstId > 0);
            Assert.True(secondId > 0);
            Assert.True(secondId > firstId);
        }

        [Fact]
        public async Task StartPaymentAsync_WithValidCommmand_MapsData()
        {
            var command = CreateValidAddPaymentCommand();
            var repository = new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance);
            var merchantId = 456;

            var id = await repository.StartPaymentAsync(merchantId, command);
            var result = await repository.GetPaymentDetailsByPaymentIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(command.Amount, result.Amount);
            Assert.Equal(command.Currency, result.Currency);
            Assert.Equal(CreditCardNumberFormatter.Mask(command.CreditCard.CardNumber), result.MaskedCreditCardNumber);
            Assert.Equal(merchantId, result.MerchantId);
            Assert.Equal(id, result.PaymentId);
            Assert.Equal(PaymentStatus.Started, result.Status);
            Assert.True(result.RequestedDate > DateTime.MinValue);
            Assert.True(!result.ResponseReceivedDate.HasValue);
            Assert.Null(result.BankPaymentId);
        }

        [Fact]
        public async Task CompletePaymentAsync_WithStatus_UpdatesRecord()
        {
            var command = CreateValidAddPaymentCommand();
            var repository = new InMemoryPaymentsRepository(NullLogger<InMemoryPaymentsRepository>.Instance);
            var bankPaymentResponse = new BankPaymentResponse()
            {
                BankPaymentId = "TEST",
                Result = PaymentStatus.Paid
            };

            var paymentId = await repository.StartPaymentAsync(1, command);
            await repository.CompletePaymentAsync(paymentId, bankPaymentResponse);
            var result = await repository.GetPaymentDetailsByPaymentIdAsync(paymentId);

            Assert.NotNull(result.BankPaymentId);
            Assert.Equal(bankPaymentResponse.Result, result.Status);
            Assert.True(result.ResponseReceivedDate > DateTime.MinValue);
        }

        private AddPaymentCommand CreateValidAddPaymentCommand()
        {
            return new AddPaymentCommand()
            {
                Amount = 32.23m,
                Currency = "EUR",
                CreditCard = CreditCardValidatorTests.CreateValidCard()
            };
        }
    }
}
