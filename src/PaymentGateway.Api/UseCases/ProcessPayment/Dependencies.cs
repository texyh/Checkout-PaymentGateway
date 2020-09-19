using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Commands;
using PaymentGateway.Framework;
using PaymentGateway.Infrastructure;
using PaymentGateway.Infrastructure.AcquiringBank;
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
            services.AddScoped<IProcessPaymentCommandHandler, ProcessPaymentCommandHandler>();
            services.TryAddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();

            if(Environment.IsDevelopment())
            {
                services.AddHostedService<AcquiringBankMockService>();
            }

            services.AddHttpClient<IAquiringBankClient, AcquiringBankClient>();

            return services;
        }
    }
}
