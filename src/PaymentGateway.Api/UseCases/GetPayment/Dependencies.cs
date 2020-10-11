using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Application.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Infrastructure;

namespace PaymentGateway.Api.UseCases.GetPayment
{
    public static class Dependencies
    {
        public static IServiceCollection AddGetPaymentUseCase(this IServiceCollection services) 
        {

            services.TryAddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();

            return services;
        }
    }
}