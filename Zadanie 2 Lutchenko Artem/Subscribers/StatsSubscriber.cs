public class StatsSubscriber
{
    public int CompletedCount = 0;

    public void OnCompleted(object sender, OrderStatusChangedEventArgs e)
    {
        if (e.NewStatus == OrderStatus.Completed)
        {
            CompletedCount++;
            Console.WriteLine($"[STATS] Completed orders: {CompletedCount}");
        }
    }
}