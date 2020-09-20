using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using PaymentGateway.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        

        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(GetConnectionString(configuration));

            services.AddDbContextPool<PaymentGatewayDbContext>((serviceProvider, builder) =>
            {
                var logger = serviceProvider.GetService<ILogger>();

                
                builder.UseNpgsql(GetConnectionString(configuration), optionsBuilder =>
                {
                   optionsBuilder.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            return services;
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = configuration.GetValue<string>("POSTGRES_HOST"),
                Port = int.Parse(configuration.GetValue<string>("POSTGRES_PORT", "5432")),
                SslMode = SslMode.Prefer,
                Username = configuration.GetValue<string>("POSTGRES_USERNAME"),
                Password = configuration.GetValue<string>("POSTGRES_PASSWORD"),
                Database = configuration.GetValue<string>("POSTGRES_DB_NAME"),
                TrustServerCertificate = true
            };

            return  connectionStringBuilder.ConnectionString;
        }
    }
}
