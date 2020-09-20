using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGateway.Infrastructure.AcquiringBank;
using ILogger = Serilog.ILogger;
using PaymentGateway.Api.Middleware;
using PaymentGateway.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AcquiringBankSettings>(options => options.ApiUrl = Configuration.GetValue("BANK_API_URL", string.Empty));
            // services.AddHealthChecksUI();
            services
                .AddSwaggerGen()
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider serviceProvider)
        {
            app.UseHsts();
            
            var logger = serviceProvider.GetService<ILogger>();
            app.UseExceptionHandler(builder => builder.HandleExceptions(logger, environment));

            logger.Information("Applying Migration");
            var dbContext = serviceProvider.GetService<PaymentGatewayDbContext>();
            dbContext.Database.Migrate();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapHealthChecksUI();
            });
        }
    }
}
