using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    /// <summary>
    /// Thrown when a resource is attempted to be accessed by a user who does not have 
    /// sufficient permissions to do so. Can be caught furthur up the chain and handled accordingly.
    /// </summary>
    /// <remarks>
    /// Copied from my code at:
    /// https://github.com/cofoundry-cms/cofoundry/blob/master/src/Cofoundry.Core/Core/Exceptions/NotPermittedException.cs
    /// </remarks>
    public class NotPermittedException : Exception
    {
        public NotPermittedException()
        {
        }
        public NotPermittedException(string message)
            : base(message)
        {
        }
    }
}
