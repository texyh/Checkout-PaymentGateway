using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Domain.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Framework;
using PaymentGateway.Infrastructure;
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
            services.AddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();
            return services;
        }
    }
}
