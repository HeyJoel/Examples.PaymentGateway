using Moq;
using Moq.Protected;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Examples.PaymentGateway.Domain.Tests
{
    public class AddPaymentCommandValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_WhenValid_Ok()
        {
            var command = CreateValidCommand();
            var validator = new AddPaymentCommandValidator();

            var result = await validator.ValidateAsync(command);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-9)]
        public async Task ValidateAsync_WhenAmountInvalid_IsInvalid(decimal amount)
        {
            var command = CreateValidCommand();
            command.Amount = amount;
            var validator = new AddPaymentCommandValidator();

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("EU")]
        [InlineData("eur")]
        [InlineData("EURO")]
        [InlineData("EU1")]
        public async Task ValidateAsync_WhenCurrencyInvalid_IsInvalid(string currency)
        {
            var command = CreateValidCommand();
            command.Currency = currency;
            var validator = new AddPaymentCommandValidator();

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_WhenCreditCardInvalid_IsInvalid()
        {
            var command = CreateValidCommand();
            command.CreditCard.CardNumber = "XYZ";
            var validator = new AddPaymentCommandValidator();

            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
        }

        public static AddPaymentCommand CreateValidCommand()
        {
            var command = new AddPaymentCommand()
            {
                Amount = 20.22m,
                CreditCard = CreditCardValidatorTests.CreateValidCard(),
                Currency = "USD"
            };

            return command;
        }
    }
}
