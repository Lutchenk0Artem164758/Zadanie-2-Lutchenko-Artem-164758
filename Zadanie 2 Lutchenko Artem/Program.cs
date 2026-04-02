using System.Diagnostics;

class Program
{
    static async Task Main()
    {
        await RunZadanie1();
        await RunZadanie2();
        RunZadanie3();
    }

    // =========================
    // ZADANIE 1
    // =========================
    static Task RunZadanie1()
    {
        Console.WriteLine("=== ZADANIE 1 ===");

        var pipeline = new OrderPipeline();

        var logger = new ConsoleLogger();
        var email = new EmailNotifier();
        var stats = new StatsSubscriber();

        pipeline.StatusChanged += logger.OnStatusChanged;
        pipeline.StatusChanged += email.OnStatusChanged;
        pipeline.StatusChanged += stats.OnCompleted;
        pipeline.ValidationCompleted += logger.OnValidation;

        var orders = new List<Order>
        {
            new Order { Id = 1, Amount = 100, Product = new Product { Name = "Laptop" }},
            new Order { Id = 2, Amount = 200, Product = new Product { Name = "Phone" }},
            new Order { Id = 3, Amount = -10, Product = new Product { Name = "Error" }}
        };

        foreach (var order in orders)
            pipeline.ProcessOrder(order);

        return Task.CompletedTask;
    }

    // =========================
    // ZADANIE 2
    // =========================
    static async Task RunZadanie2()
    {
        Console.WriteLine("\n=== ZADANIE 2 ===");

        var service = new ExternalServiceSimulator();

        var orders = new List<Order>
        {
            new Order { Id = 1, Amount = 100, Product = new Product { Name = "Laptop" }},
            new Order { Id = 2, Amount = 200, Product = new Product { Name = "Phone" }},
            new Order { Id = 3, Amount = 300, Product = new Product { Name = "Tablet" }},
            new Order { Id = 4, Amount = 400, Product = new Product { Name = "TV" }},
            new Order { Id = 5, Amount = 500, Product = new Product { Name = "Mouse" }},
            new Order { Id = 6, Amount = 600, Product = new Product { Name = "Keyboard" }}
        };

        async Task ProcessOrderAsync(Order order)
        {
            var sw = Stopwatch.StartNew();

            var t1 = service.CheckInventoryAsync(order.Product);
            var t2 = service.ValidatePaymentAsync(order);
            var t3 = service.CalculateShippingAsync(order);

            await Task.WhenAll(t1, t2, t3);

            sw.Stop();
            Console.WriteLine($"Order {order.Id} done in {sw.ElapsedMilliseconds} ms");
        }

        // 🔹 Sequential
        var swSeq = Stopwatch.StartNew();
        foreach (var o in orders)
            await ProcessOrderAsync(o);
        swSeq.Stop();

        Console.WriteLine($"Sequential time: {swSeq.ElapsedMilliseconds} ms");

        // 🔹 Parallel (max 3)
        var semaphore = new SemaphoreSlim(3);
        int done = 0;

        var swPar = Stopwatch.StartNew();

        var tasks = orders.Select(async o =>
        {
            await semaphore.WaitAsync();

            try
            {
                await ProcessOrderAsync(o);
                Console.WriteLine($"Progress: {Interlocked.Increment(ref done)}/{orders.Count}");
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
        swPar.Stop();

        Console.WriteLine($"Parallel time: {swPar.ElapsedMilliseconds} ms");
    }

    // =========================
    // ZADANIE 3
    // =========================
    static void RunZadanie3()
    {
        Console.WriteLine("\n=== ZADANIE 3 ===");

        var orders = Enumerable.Range(1, 1000)
            .Select(i => new Order { Id = i, Amount = 10, Status = OrderStatus.Completed })
            .ToList();

        //  unsafe
        var unsafeStats = new OrderStatisticsUnsafe();

        Parallel.ForEach(orders, o =>
        {
            unsafeStats.Add(o);
        });

        Console.WriteLine($"UNSAFE: {unsafeStats.TotalProcessed} | {unsafeStats.TotalRevenue}");

        //  safe
        var safeStats = new OrderStatistics();

        Parallel.ForEach(orders, o =>
        {
            safeStats.Add(o);
        });

        Console.WriteLine($"SAFE: {safeStats.TotalProcessed} | {safeStats.TotalRevenue}");
    }
}
