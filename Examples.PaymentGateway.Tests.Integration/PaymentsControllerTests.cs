using Examples.PaymentGateway;
using Examples.PaymentGateway.Domain;
using Examples.PaymentGateway.Domain.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Examples.PaymentGateway.Tests.Integration
{
    public class PaymentsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public PaymentsControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task Get_WithNonExistingId_Returns404()
        {
            var client = _webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/api/payments/123");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsResult()
        {
            var client = _webApplicationFactory.CreateClient();
            AddPaymentCommandResult paymentResult = null;
            using (var scope = _webApplicationFactory.Server.Host.Services.CreateScope())
            {
                var command = AddPaymentCommandValidatorTests.CreateValidCommand();
                var addPaymentCommandHandler = scope.ServiceProvider.GetRequiredService<AddPaymentCommandHandler>();
                paymentResult = await addPaymentCommandHandler.ExecuteAsync(command);
            }

            var response = await client.GetAsync("/api/payments/" + paymentResult.PaymentId);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var payment = JsonConvert.DeserializeObject<PaymentDetails>(json);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(payment);
            Assert.Equal(paymentResult.PaymentId, payment.PaymentId);
        }

        [Fact]
        public async Task Post_WithValidCommand_ReturnsResult()
        {
            var client = _webApplicationFactory.CreateClient();
            var command = AddPaymentCommandValidatorTests.CreateValidCommand();

            var response = await client.PostAsJsonAsync("/api/payments/", command);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var payment = JsonConvert.DeserializeObject<AddPaymentCommandResult>(json);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(payment);
            Assert.True(payment.PaymentId > 0);
            Assert.NotEqual(PaymentStatus.NotStarted, payment.PaymentResult);
            Assert.NotEqual(PaymentStatus.Started, payment.PaymentResult);
        }

        [Fact]
        public async Task Post_WithValidInvalidCommand_Returns400()
        {
            var client = _webApplicationFactory.CreateClient();
            var command = AddPaymentCommandValidatorTests.CreateValidCommand();
            command.Currency = null;

            var response = await client.PostAsJsonAsync("/api/payments/", command);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
