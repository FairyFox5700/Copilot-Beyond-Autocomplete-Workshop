namespace Store.Checkout;

public sealed record CheckoutBreakdown(
    decimal MerchandiseSubtotal,
    decimal Shipping,
    decimal LoyaltyDiscount,
    decimal Total);

public sealed class CheckoutCalculator
{
    public CheckoutBreakdown Calculate(
        decimal merchandiseSubtotal,
        decimal shipping,
        decimal loyaltyDiscountRate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(merchandiseSubtotal);
        ArgumentOutOfRangeException.ThrowIfNegative(shipping);

        if (loyaltyDiscountRate is < 0m or > 1m)
        {
            throw new ArgumentOutOfRangeException(nameof(loyaltyDiscountRate));
        }

        var discount = decimal.Round(
            merchandiseSubtotal * loyaltyDiscountRate,
            2,
            MidpointRounding.AwayFromZero);

        return new CheckoutBreakdown(
            merchandiseSubtotal,
            shipping,
            discount,
            merchandiseSubtotal + shipping - discount);
    }
}
