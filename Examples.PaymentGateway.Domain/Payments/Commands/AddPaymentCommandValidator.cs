using FluentValidation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain
{
    public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
    {
        public AddPaymentCommandValidator()
        {
            RuleFor(o => o.Amount)
                .GreaterThan(0);

            RuleFor(o => o.CreditCard)
                .SetValidator(new CreditCardValidator());

            // Assuming all currencies are accepted
            // We could be more liberal here and accept lowercase / convert
            RuleFor(o => o.Currency)
                .NotEmpty()
                .Matches("^[A-Z]{3}$");;
        }
    }
}
