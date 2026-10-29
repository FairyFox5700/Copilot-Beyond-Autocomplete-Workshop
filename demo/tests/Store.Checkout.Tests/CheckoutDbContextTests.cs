using Microsoft.EntityFrameworkCore;
using Store.Checkout.Persistence;

namespace Store.Checkout.Tests;

public sealed class CheckoutDbContextTests
{
    [Fact]
    public void CheckoutOrder_ModelRequiresCustomerAndTotal()
    {
        var options = new DbContextOptionsBuilder<CheckoutDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new CheckoutDbContext(options);
        var entity = context.Model.FindEntityType(typeof(CheckoutOrder));

        Assert.NotNull(entity);
        Assert.False(entity.FindProperty(nameof(CheckoutOrder.CustomerEmail))!.IsNullable);
        Assert.False(entity.FindProperty(nameof(CheckoutOrder.Total))!.IsNullable);
        Assert.Equal("decimal(18,2)", entity.FindProperty(nameof(CheckoutOrder.Total))!.GetColumnType());
    }
}
