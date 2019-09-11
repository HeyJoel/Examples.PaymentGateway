using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    /// <summary>
    /// Assumption: The API would have some kind of authentication
    /// that would identify the merchant making the payment request.
    /// I also assume that the system would know how to pay the merchant 
    /// based on this identifier i.e. I've not included merchant payment
    /// details.
    /// </summary>
    public interface IUserSessionService
    {
        /// <summary>
        /// Returns the merchant id of the currently logged
        /// in user.
        /// </summary>
        int GetCurrentMerchantId();
    }
}
