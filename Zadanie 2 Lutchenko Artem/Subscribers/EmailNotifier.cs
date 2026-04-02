public class EmailNotifier
{
    public void OnStatusChanged(object sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"[EMAIL] Order {e.Order.Id} changed to {e.NewStatus}");
    }
}