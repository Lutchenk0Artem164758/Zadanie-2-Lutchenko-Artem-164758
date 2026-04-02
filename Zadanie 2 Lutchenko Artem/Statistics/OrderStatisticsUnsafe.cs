public class OrderStatisticsUnsafe
{
    public int TotalProcessed = 0;
    public decimal TotalRevenue = 0;
    public Dictionary<OrderStatus, int> OrdersPerStatus = new();

    public void Add(Order order)
    {
        TotalProcessed++;
        TotalRevenue += order.Amount;

        if (!OrdersPerStatus.ContainsKey(order.Status))
            OrdersPerStatus[order.Status] = 0;

        OrdersPerStatus[order.Status]++;
    }
}