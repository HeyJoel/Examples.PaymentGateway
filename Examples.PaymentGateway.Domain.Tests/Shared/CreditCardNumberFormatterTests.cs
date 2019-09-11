using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Examples.PaymentGateway.Domain.Tests
{
    public class CreditCardNumberFormatterTests
    {
        const string VISA_CARD_NUMBER = "4012888888881881";
        const string AMEX_CARD_NUMBER = "371449635398431";
        const string VISA_13_DIGIT_CARD_NUMBER = "4222222222222";

        [Theory]
        [InlineData(AMEX_CARD_NUMBER)]
        [InlineData(VISA_CARD_NUMBER)]
        [InlineData(VISA_13_DIGIT_CARD_NUMBER)]
        public void ToDigitsOnly_WhenValid_ReturnsUnchanged(string cardNumber)
        {
            var result = CreditCardNumberFormatter.ToDigitsOnly(cardNumber);

            Assert.Equal(cardNumber, result);
        }

        [Theory]
        [InlineData("4222-2222-22222", VISA_13_DIGIT_CARD_NUMBER)]
        [InlineData("4012–8888–8888–1881", VISA_CARD_NUMBER)]
        [InlineData("3714 496353 98431", AMEX_CARD_NUMBER)]
        [InlineData(" 4012–8888–8888–1881 ", VISA_CARD_NUMBER)]
        public void ToDigitsOnly_WithNonDigits_ReturnsNormalized(string cardNumber, string expected)
        {
            var result = CreditCardNumberFormatter.ToDigitsOnly(cardNumber);

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("4222-2222-22222", "XXXXXXXXX2222")]
        [InlineData("4012–8888–8888–1881", "XXXXXXXXXXXX1881")]
        [InlineData("3714 496353 98431", "XXXXXXXXXXX8431")]
        [InlineData(" 4012–8888–8888–1881 ", "XXXXXXXXXXXX1881")]
        public void Mask_WithValidValue_ReturnsMasked(string cardNumber, string expected)
        {
            var result = CreditCardNumberFormatter.Mask(cardNumber);

            Assert.Equal(expected, result);
        }

    }
}
