namespace ReturnValueTask
{
    internal class Program
    {
        public const string ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=AdventureWorks; Integrated Security=true;MultipleActiveResultSets=False;";


        //private static async Task Main(string[] args)
        //{
        //    var customerService = new CustomerService(ConnectionString);
        //    var c = await customerService.GetCustomersWithTaskAsync("Mr.");
        //    var c2 = await customerService.GetCustomersWithTaskAsync("Mr.");
        //    Console.WriteLine($"Count: {c}");
        //}


        private static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<CustomerServiceBenchmark>();
        }

    }
}
