using System.Collections.Concurrent;

public class OrderStatistics
{
    public int TotalProcessed = 0;
    public decimal TotalRevenue = 0;

    public ConcurrentDictionary<OrderStatus, int> OrdersPerStatus = new();
    public List<string> Errors = new();

    private object _lock = new object();

    public void Add(Order order)
    {
        Interlocked.Increment(ref TotalProcessed);

        lock (_lock)
        {
            TotalRevenue += order.Amount;
        }

        OrdersPerStatus.AddOrUpdate(order.Status, 1, (_, old) => old + 1);
    }

    public void AddError(string error)
    {
        lock (_lock)
        {
            Errors.Add(error);
        }
    }
}