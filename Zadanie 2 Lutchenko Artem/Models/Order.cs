public class Order
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public Product Product { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.New;
}