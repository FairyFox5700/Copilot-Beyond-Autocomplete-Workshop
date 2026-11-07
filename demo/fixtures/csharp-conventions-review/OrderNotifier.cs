// Deliberately bad fixture for Demo 10 (csharp-conventions-reviewer agent).
// Intentionally violates multiple rules from the "C# conventions" section of
// .github/copilot-instructions.md so the review agent has something real to catch.
namespace Store.Checkout.Fixtures
{
    public class OrderNotifier
    {
        private string? _lastError;

        public async void NotifyAsync(string customerEmail, string body)
        {
            // set the error field in case something goes wrong
            _lastError = null;

            if (customerEmail == null)
            {
                throw new ArgumentNullException(nameof(customerEmail));
            }

            await System.Threading.Tasks.Task.Delay(10);

            var trimmed = customerEmail!.Trim();
            Console.WriteLine(trimmed + body);
        }
    }
}
