namespace ReturnTaskOnly
{
    public class OrderService
    {



        public Task<string> GetOrders_NotAwaited_Task()
        {
            return Task.FromResult("");

            return File.ReadAllTextAsync("orders1.json");

            // using orders4.json to simulate an error
            // return File.ReadAllTextAsync("orders4.json");

            // BAD: don't use with using
            //using var file = new StreamReader("orders1.json", new FileStreamOptions { Access = FileAccess.ReadWrite });
            //return file.ReadToEndAsync();


        }

        public async Task<string> GetOrders_Awaited_Task()
        {
            return await Task.FromResult("");

            return await File.ReadAllTextAsync("orders2.json");

            // using orders4.json to simulate an error
            //return await File.ReadAllTextAsync("orders4.json");

            // GOOD: use with using
            //using var file = new StreamReader("orders2.json", new FileStreamOptions { Access = FileAccess.ReadWrite });
            //return await file.ReadToEndAsync();

        }

    }
}
