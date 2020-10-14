using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Application.Crypto;
using PaymentGateway.Domain.Abstractions;
using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Commands;
using PaymentGateway.Infrastructure;
using PaymentGateway.Infrastructure.AcquiringBank;
using PaymentGateway.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.UseCases.ProcessPayment
{
    public static class Dependencies
    {
        public static IServiceCollection AddProcessPaymentUseCase(this IServiceCollection services)
        {
            services.TryAddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();
            services.TryAddScoped(typeof(IAggregateStore<>), typeof(AggreateStore<>));



            if (Environment.IsDevelopment())
            {
                services.AddHostedService<AcquiringBankMockService>();
            }

            services.AddHttpClient<IAquiringBankClient, AcquiringBankClient>()
                    .AddPolicyHandler(RetryPolicy.GetRetryPolicy(2));

            return services;
        }
    }
}
