using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Api.Application.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Queries;
using PaymentGateway.Infrastructure;

namespace PaymentGateway.Api.UseCases.GetPayment
{
    public static class Dependencies
    {
        public static IServiceCollection AddGetPaymentUseCase(this IServiceCollection services) 
        {

            services.AddScoped<IGetPaymentQueryHandler, GetPaymentQueryHandler>();
            services.TryAddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();

            return services;
        }
    }
}