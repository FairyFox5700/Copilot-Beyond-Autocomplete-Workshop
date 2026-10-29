using Store.Checkout;

namespace Store.Checkout.Tests;

public sealed class CheckoutCalculatorTests
{
    private readonly CheckoutCalculator _calculator = new();

    [Fact]
    public void LoyaltyDiscount_AppliesToMerchandiseButNotShipping()
    {
        var result = _calculator.Calculate(100m, 10m, 0.10m);

        Assert.Equal(10m, result.LoyaltyDiscount);
        Assert.Equal(100m, result.Total);
    }

    [Fact]
    public void NoDiscount_ReturnsSubtotalPlusShipping()
    {
        var result = _calculator.Calculate(25m, 5m, 0m);

        Assert.Equal(0m, result.LoyaltyDiscount);
        Assert.Equal(30m, result.Total);
    }

    [Fact]
    public void Discount_RoundsAwayFromZeroToCurrencyPrecision()
    {
        var result = _calculator.Calculate(19.99m, 0m, 0.10m);

        Assert.Equal(2.00m, result.LoyaltyDiscount);
        Assert.Equal(17.99m, result.Total);
    }
}
