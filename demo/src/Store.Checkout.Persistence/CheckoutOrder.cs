namespace Store.Checkout.Persistence;

public sealed class CheckoutOrder
{
    public Guid Id { get; set; }

    public required string CustomerEmail { get; set; }

    public decimal Total { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}
