using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    /// <summary>
    /// Mocked because authentication functionality in not in 
    /// scope for this example.
    /// </summary>
    public class MockUserSessionService : IUserSessionService
    {
        public int GetCurrentMerchantId()
        {
            return 1;
        }
    }
}
