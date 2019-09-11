using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Domain.Internal
{
    /// <summary>
    /// Abstraction of the out-of-scope aquiring bank
    /// service.
    /// </summary>
    /// <remarks>
    /// It is assumed that this service will know how to 
    /// send the merchants payment details along to the 
    /// aquiring bank, as this is considered out of scope
    /// for the example.
    /// </remarks>
    public interface IAquiringBankService
    {
        Task<BankPaymentResponse> MakePaymentAsync(AddBankPaymentCommand command);
    }
}
