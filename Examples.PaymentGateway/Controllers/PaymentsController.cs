using Examples.PaymentGateway.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.PaymentGateway.Controllers
{
    /// <summary>
    /// The API controllers are a thin layer over the domain layer.
    /// The domain layer is invoked via individual query and commands 
    /// handlers - a simpliied version of that you see in a CQS framework 
    /// like MediatR.
    /// </summary>
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly GetPaymentDetailsByPaymentIdQueryHandler _getPaymentDetailsByTokenQueryHandler;
        private readonly AddPaymentCommandHandler _addPaymentCommandHandler;

        public PaymentsController(
            GetPaymentDetailsByPaymentIdQueryHandler getPaymentDetailsByTokenQueryHandler,
            AddPaymentCommandHandler addPaymentCommandHandler
            )
        {
            _getPaymentDetailsByTokenQueryHandler = getPaymentDetailsByTokenQueryHandler;
            _addPaymentCommandHandler = addPaymentCommandHandler;
        }

        [Route("{id:int}")]
        public async Task<ActionResult<PaymentDetails>> GetById(int id)
        {
            var query = new GetPaymentDetailsByPaymentIdQuery(id);

            var paymentResult = await _getPaymentDetailsByTokenQueryHandler.ExecuteAsync(query);

            if (paymentResult == null)
            {
                return NotFound();
            }

            return paymentResult;
        }

        [HttpPost]
        public async Task<AddPaymentCommandResult> Post([FromBody] AddPaymentCommand command)
        {
            if (ModelState.IsValid)
            {
                return await _addPaymentCommandHandler.ExecuteAsync(command);
            }

            return null;
        }
    }
}
