public class OrderPipeline
{
    public event EventHandler<OrderStatusChangedEventArgs> StatusChanged;
    public event EventHandler<OrderValidationEventArgs> ValidationCompleted;

    public void ProcessOrder(Order order)
    {
        Validate(order);

        ChangeStatus(order, OrderStatus.Validated);
        ChangeStatus(order, OrderStatus.Processing);
        ChangeStatus(order, OrderStatus.Completed);
    }

    private void Validate(Order order)
    {
        var errors = new List<string>();

        if (order.Amount <= 0)
            errors.Add("Amount must be greater than 0");

        bool isValid = errors.Count == 0;

        ValidationCompleted?.Invoke(this, new OrderValidationEventArgs
        {
            Order = order,
            IsValid = isValid,
            Errors = errors
        });
    }

    private void ChangeStatus(Order order, OrderStatus newStatus)
    {
        var oldStatus = order.Status;
        order.Status = newStatus;

        StatusChanged?.Invoke(this, new OrderStatusChangedEventArgs
        {
            Order = order,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Timestamp = DateTime.Now
        });
    }
}