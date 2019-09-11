using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    public static class CreditCardNumberFormatter
    {
        /// <summary>
        /// Masks a credit card number in the format "XXXXXXXXXXXX1234".
        /// </summary>
        /// <remarks>
        /// An assumption is made on the formatting of the masked value. The 
        /// card number  length is preserved, but to avoid edge cases it could 
        /// be simpler to just display the last four digits.
        /// </remarks>
        /// <param name="creditCardNumber">
        /// The credit card number to mask. Cannot be null and must be a valid
        /// credit card number length, but can include spaces or dashes.
        /// </param>
        public static string Mask(string creditCardNumber)
        {
            const string MASKED_PREFIX = "XXXXXXXXXXXXXXX";

            if (creditCardNumber == null) throw new ArgumentNullException(nameof(creditCardNumber));

            var normalized = ToDigitsOnly(creditCardNumber);

            if (normalized.Length > 19 || normalized.Length < 12)
            {
                throw new ArgumentOutOfRangeException($"Invalid card number length: {normalized.Length}.");
            }

            var maskIndex = normalized.Length - 4;
            var maskedNumber = MASKED_PREFIX.Remove(maskIndex) + normalized.Substring(maskIndex);

            return maskedNumber;
        }

        /// <summary>
        /// Normalizes a credit card number removing anything other
        /// than numbers (e.g. dashes or whitespace).
        /// </summary>
        /// <param name="creditCardNumber">
        /// The credit card number to normalize. Cannot be null.
        /// </param>
        public static string ToDigitsOnly(string creditCardNumber)
        {
            if (creditCardNumber == null) throw new ArgumentNullException(nameof(creditCardNumber));

            if (!creditCardNumber.Any(c => !Char.IsDigit(c)))
            {
                return creditCardNumber;
            }

            // there's probably something fun we can do with Span<T> here, but
            // I've not got time to look it up.
            var numbers = creditCardNumber
                .Where(c => Char.IsDigit(c))
                .ToArray();

            var normalized = new string(numbers);

            return normalized;
        }
    }
}
