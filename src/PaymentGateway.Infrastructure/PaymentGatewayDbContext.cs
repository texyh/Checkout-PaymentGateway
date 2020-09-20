using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Infrastructure
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options)
        {

        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().ToTable("Payments").HasKey(x => x.Id);
        }
    }
}
