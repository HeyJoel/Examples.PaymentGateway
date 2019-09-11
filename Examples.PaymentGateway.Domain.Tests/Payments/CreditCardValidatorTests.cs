using Moq;
using Moq.Protected;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Examples.PaymentGateway.Domain.Tests
{
    public class CreditCardValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_WhenValid_Ok()
        {
            var card = CreateValidCard();
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("4999111111111111")]
        [InlineData("41111111111111111")]
        [InlineData("411111111111111")]
        public async Task ValidateAsync_WhenCardNumberInvalid_IsInvalid(string cardNumber)
        {
            var card = CreateValidCard();
            card.CardNumber = cardNumber;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("371449635398431")]
        [InlineData("378734493671000")]
        [InlineData("6331101999990016")]
        [InlineData("4012888888881881")]
        [InlineData("4222222222222")]
        public async Task ValidateAsync_WhenCardNumberValid_IsValid(string cardNumber)
        {
            var card = CreateValidCard();
            card.CardNumber = cardNumber;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("01")]
        [InlineData("01100")]
        [InlineData("1A2")]
        public async Task ValidateAsync_WhenCCVInvalid_IsInvalid(string ccv)
        {
            var card = CreateValidCard();
            card.CCV = ccv;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("0110")]
        [InlineData("1234")]
        public async Task ValidateAsync_WhenCCV4Digits_IsValid(string ccv)
        {
            var card = CreateValidCard();
            card.CCV = ccv;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(13)]
        public async Task ValidateAsync_WhenExpiryMonthOutOfBounds_IsInvalid(int expiryMonth)
        {
            var card = CreateValidCard();
            card.ExpiryMonth = expiryMonth;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2000)]
        [InlineData(2018)]
        public async Task ValidateAsync_WhenExpiryYearInvalid_IsInvalid(int expiryYear)
        {
            var card = CreateValidCard();
            card.ExpiryYear = expiryYear;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_ExpiringThisMonth_IsValid()
        {
            var expiryDate = new DateTime(2010, 3, 31);
            // allow ambiguity for UTC -12 timezone
            var dateTimeToTest = new DateTime(2010, 3, 31, 23, 59, 59).AddHours(12);

            var card = CreateValidCard();
            card.ExpiryYear = expiryDate.Year;
            card.ExpiryMonth = expiryDate.Month;

            var validator = new Mock<CreditCardValidator>();
            validator.CallBase = true;
            validator
                .Protected()
                .Setup<DateTime>("GetUtcNow")
                .Returns(dateTimeToTest);

            var result = await validator.Object.ValidateAsync(card);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_ExpiredLastMonth_IsInvalid()
        {
            var expiryDate = new DateTime(2010, 3, 31);
            // allow ambiguity for UTC -12 timezone
            var dateTimeToTest = new DateTime(2010, 4, 1, 00, 00, 00).AddHours(12);

            var card = CreateValidCard();
            card.ExpiryYear = expiryDate.Year;
            card.ExpiryMonth = expiryDate.Month;

            var validator = new Mock<CreditCardValidator>();
            validator.CallBase = true;
            validator
                .Protected()
                .Setup<DateTime>("GetUtcNow")
                .Returns(dateTimeToTest);

            var result = await validator.Object.ValidateAsync(card);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateAsync_WhenNameOnCardInvalid_IsInvalid(string name)
        {
            var card = CreateValidCard();
            card.NameOnCard = name;
            var validator = new CreditCardValidator();

            var result = await validator.ValidateAsync(card);

            Assert.False(result.IsValid);
        }

        public static CreditCard CreateValidCard()
        {
            var command = new CreditCard()
            {
                CardNumber = "4111111111111111",
                CCV = "123",
                ExpiryMonth = 4,
                ExpiryYear = DateTime.UtcNow.AddYears(1).Year,
                NameOnCard = "Dr D Dobbs"
            };

            return command;
        }
    }
}
