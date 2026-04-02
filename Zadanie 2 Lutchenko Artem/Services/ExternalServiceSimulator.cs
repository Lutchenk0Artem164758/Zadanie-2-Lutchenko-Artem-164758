public class ExternalServiceSimulator
{
    private static Random rnd = Random.Shared;

    public async Task<bool> CheckInventoryAsync(Product product)
    {
        await Task.Delay(rnd.Next(500, 1500));
        return rnd.Next(0, 2) == 1;
    }

    public async Task<bool> ValidatePaymentAsync(Order order)
    {
        await Task.Delay(rnd.Next(1000, 2000));
        return rnd.Next(0, 2) == 1;
    }

    public async Task<decimal> CalculateShippingAsync(Order order)
    {
        await Task.Delay(rnd.Next(300, 800));
        return rnd.Next(10, 50);
    }
}