namespace ReturnTaskOnly
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            BenchmarkDotNet.Running.BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig());
            //await HandlingExceptionsAsync();

#else

            BenchmarkDotNet.Running.BenchmarkRunner.Run<TaskReturningTaskBenchmark>();
#endif
        }


        /// <summary>
        /// See how the error stack trace is different when using await and not using await
        /// </summary>
        /// <returns></returns>
        private static async Task HandlingExceptionsAsync()
        {
            var orderService = new OrderService();

            Console.WriteLine("-------------------------------");
            try
            {
                var orders = await orderService.GetOrders_Awaited_Task();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("-------------------------------");
            try
            {
                var orders = await orderService.GetOrders_NotAwaited_Task();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("-------------------------------");
        }
    }
}
