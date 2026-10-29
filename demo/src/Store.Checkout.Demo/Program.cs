using Store.Checkout;

var calculator = new CheckoutCalculator();
var result = calculator.Calculate(
    merchandiseSubtotal: 100m,
    shipping: 10m,
    loyaltyDiscountRate: 0.10m);

Console.WriteLine("Checkout: loyalty customer");
Console.WriteLine($"Merchandise: {result.MerchandiseSubtotal:C}");
Console.WriteLine($"Shipping:    {result.Shipping:C}");
Console.WriteLine($"Discount:   -{result.LoyaltyDiscount:C}");
Console.WriteLine($"Total:       {result.Total:C}");
Console.WriteLine();
Console.WriteLine("Contract: shipping is never discountable");
Console.WriteLine("Expected discount: $10.00 | Expected total: $100.00");
