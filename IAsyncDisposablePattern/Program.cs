namespace IAsyncDisposablePattern
{
    internal class Program
    {
        public const string ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=AdventureWorks; Integrated Security=true;MultipleActiveResultSets=False;";

        private static async Task Main(string[] args)
        {
            var customerService = new CustomerService(ConnectionString);
            var customers = await customerService.GetCustomersAsync("Mr.");
        }

        private static async Task AwaitUsingTaskSample()
        {
            await using (var fileStreamText = new FileStream("file.txt", FileMode.Create))
            {
                // Do something with the fileStream
            }

            // Be careful when using with ConfigureAwait(false)
            // You should first create the instance then use ConfigureAwait(false)
            var fileStream = new FileStream("file.txt", FileMode.Create);
            await using (fileStream.ConfigureAwait(false))
            {
                // Do something with the fileStream
            }

        }
    }
}
