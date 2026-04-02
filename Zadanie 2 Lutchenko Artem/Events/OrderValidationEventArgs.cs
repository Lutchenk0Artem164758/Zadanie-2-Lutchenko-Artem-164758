public class OrderValidationEventArgs : EventArgs
{
    public Order Order { get; set; }
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
}