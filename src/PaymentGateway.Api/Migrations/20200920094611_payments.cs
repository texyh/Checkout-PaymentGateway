using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Api.Migrations
{
    public partial class payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CardNumber = table.Column<string>(nullable: true),
                    CardExpiryMonth = table.Column<string>(nullable: true),
                    CardExpiryYear = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    CVV = table.Column<string>(nullable: true),
                    EncriptionKey = table.Column<string>(nullable: true),
                    BankPaymentIdentifier = table.Column<string>(nullable: true),
                    MerchantId = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
