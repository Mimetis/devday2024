using BenchmarkDotNet.Attributes;

namespace ReturnTaskOnly
{
    [MemoryDiagnoser]
    public class TaskReturningTaskBenchmark
    {
        private OrderService orderService = new();

        [Benchmark]
        public async Task<string> Task_Awaited()
        {
            return await orderService.GetOrders_Awaited_Task();
        }

        [Benchmark]
        public async Task<string> Task_Not_Awaited()
        {
            return await orderService.GetOrders_NotAwaited_Task();
        }

    }
}
