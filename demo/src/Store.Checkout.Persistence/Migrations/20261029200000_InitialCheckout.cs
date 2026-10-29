using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Checkout.Persistence.Migrations;

public partial class InitialCheckout : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CheckoutOrders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CustomerEmail = table.Column<string>(type: "TEXT", maxLength: 320, nullable: false),
                Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CheckoutOrders", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "CheckoutOrders");
    }
}
