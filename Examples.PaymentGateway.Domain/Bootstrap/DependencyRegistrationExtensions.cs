using Examples.PaymentGateway.Domain.Internal;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examples.PaymentGateway.Domain
{
    /// <summary>
    /// Modular registration of dependencies for this assembly.
    /// </summary>
    public static class DependencyRegistrationExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services
                .AddTransient<AddPaymentCommandHandler>()
                .AddTransient<GetPaymentDetailsByPaymentIdQueryHandler>()
                .AddTransient<IUserSessionService, MockUserSessionService>()
                .AddSingleton<IAquiringBankService, MockAquiringBankService>()
                .AddSingleton<IPaymentRepository, InMemoryPaymentsRepository>()
                ;

            return services;
        }

        public static FluentValidationMvcConfiguration RegisterValidatorsFromDomain(this FluentValidationMvcConfiguration configuration)
        {
            configuration.RegisterValidatorsFromAssemblyContaining(typeof(DependencyRegistrationExtensions));

            return configuration;
        }
    }
}
