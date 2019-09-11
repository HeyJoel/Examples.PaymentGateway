using FluentValidation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain
{
    public class CreditCardValidator : AbstractValidator<CreditCard>
    {
        public CreditCardValidator()
        {
            // We could also check for specific card types we accepted
            RuleFor(o => o.CardNumber)
                .NotEmpty()
                .CreditCard();

            // We could also validate 3 or 4 digit length depending on card type
            RuleFor(o => o.CCV)
                .NotEmpty()
                .Matches("^[0-9]{3,4}$");

            RuleFor(o => o.ExpiryMonth)
                .GreaterThan(0)
                .LessThan(13);
            
            RuleFor(o => o.ExpiryYear)
                .Must(HasExpired);

            // Arbitrary max-length
            RuleFor(o => o.NameOnCard)
                .NotEmpty()
                .MaximumLength(300);
        }

        private bool HasExpired(CreditCard creditCard, int year)
        {
            // Not sure what timezone the expiry date applies to, so let's
            // defer tha decision to the aquiring bank on edge cases.
            var dateNow = GetUtcNow().AddHours(-12);

            if (year == dateNow.Year)
            {
                return creditCard.ExpiryMonth >= dateNow.Month;
            }

            return year > dateNow.Year;
        }

        /// <summary>
        /// Overridable seam for testing date-dependent validators
        /// </summary>
        protected virtual DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
