using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PaymentGateway.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway Api",
                    Version = "v1",
                    Description = "This is an api that allow merchants process and manage payments"
                });
            });

            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services)
        {
            services.AddSingleton(_ => new DbContext());

            return services;
        }
    }
}
