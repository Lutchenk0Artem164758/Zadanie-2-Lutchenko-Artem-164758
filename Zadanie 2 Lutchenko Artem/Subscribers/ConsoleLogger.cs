public class ConsoleLogger
{
    public void OnStatusChanged(object sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"[LOG] Order {e.Order.Id}: {e.OldStatus} → {e.NewStatus}");
    }

    public void OnValidation(object sender, OrderValidationEventArgs e)
    {
        Console.WriteLine($"[VALIDATION] Order {e.Order.Id}: {(e.IsValid ? "OK" : "FAILED")}");
    }
}