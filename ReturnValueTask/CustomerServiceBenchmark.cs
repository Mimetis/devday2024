using BenchmarkDotNet.Attributes;

namespace ReturnValueTask
{
    [MemoryDiagnoser]
    public class CustomerServiceBenchmark
    {
        private readonly CustomerService customerService = new(Program.ConnectionString);


        [Benchmark]
        public async Task<IList<Customer>> GetCountWithTask()
        {
            return await customerService.GetCustomersWithTaskAsync("Mr.");
        }

        [Benchmark]
        public async Task<IList<Customer>> GetCountWithTask2()
        {
            return await customerService.GetCustomersWithTaskAsync("Mr.");
        }

        [Benchmark]
        public async ValueTask<IList<Customer>> GetCountWithValueTask()
        {
            return await customerService.GetCustomersWithValueTaskAsync("Mr.");
        }

        [Benchmark]
        public async ValueTask<IList<Customer>> GetCountWithValueTask2()
        {
            return await customerService.GetCustomersWithValueTaskAsync("Mr.");
        }
    }
}
