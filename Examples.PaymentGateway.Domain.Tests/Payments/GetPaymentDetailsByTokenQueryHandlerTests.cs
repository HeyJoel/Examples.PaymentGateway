using Examples.PaymentGateway.Domain.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Examples.PaymentGateway.Domain.Tests
{
    public class GetPaymentDetailsByTokenQueryHandlerTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidPaymentId_ReturnsResult()
        {
            var paymentId = 123;
            var merchantId = 987;

            var userSessionServiceMock = new Mock<IUserSessionService>();
            userSessionServiceMock
                .Setup(m => m.GetCurrentMerchantId())
                .Returns(merchantId);

            var paymentsRepositoryMock = new Mock<IPaymentRepository>();
            paymentsRepositoryMock
                .Setup(m => m.GetPaymentDetailsByPaymentIdAsync(It.Is<int>(v => v == paymentId)))
                .ReturnsAsync(new PaymentDetails()
                {
                    PaymentId = paymentId,
                    MerchantId = merchantId
                });

            var queryHandler = new GetPaymentDetailsByPaymentIdQueryHandler(userSessionServiceMock.Object, paymentsRepositoryMock.Object);

            var query = new GetPaymentDetailsByPaymentIdQuery(paymentId);
            var result = await queryHandler.ExecuteAsync(query);

            Assert.NotNull(result);
            Assert.Equal(paymentId, result.PaymentId);
        }

        [Fact]
        public Task ExecuteAsync_WithInvalidMerchantId_ThrowsNotPermittedException()
        {
            var paymentId = 123;
            var merchantId = 999;

            var userSessionServiceMock = new Mock<IUserSessionService>();
            userSessionServiceMock
                .Setup(m => m.GetCurrentMerchantId())
                .Returns(merchantId);

            var paymentsRepositoryMock = new Mock<IPaymentRepository>();
            paymentsRepositoryMock
                .Setup(m => m.GetPaymentDetailsByPaymentIdAsync(It.Is<int>(v => v == paymentId)))
                .ReturnsAsync(new PaymentDetails()
                {
                    PaymentId = paymentId,
                    MerchantId = 111
                });

            var query = new GetPaymentDetailsByPaymentIdQuery(paymentId);
            var queryHandler = new GetPaymentDetailsByPaymentIdQueryHandler(userSessionServiceMock.Object, paymentsRepositoryMock.Object);

            return Assert.ThrowsAsync<NotPermittedException>(() => queryHandler.ExecuteAsync(query));
        }
    }
}
