namespace AsyncVoidError
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                DoStuff();
                Console.WriteLine("We are continuing coz we don't care about what is going on in DoStuff.");
            }
            catch (Exception)
            {
                Console.WriteLine("An exception occured, but we are fine ....");
            }
            Console.ReadLine();

        }


        public static async void DoStuff()
        {
            throw new InvalidOperationException("Throwing exception as invalid operation exception");
            await Task.Delay(2000);

            Console.WriteLine("Fire.");

        }




        //private static async Task Main(string[] args)
        //{
        //    DoStuff()
        //        .ContinueWith((t) => Console.WriteLine("We are after that DoStuff stack"), TaskContinuationOptions.NotOnFaulted)
        //        .FireAndForgetSafeAsync((ex) => Console.WriteLine("An exception occured, but we are fine ...."));
        //    Console.WriteLine("We are continuing coz we don't care about what is going on in DoStuff");
        //    Console.ReadLine();

        //}
    }
}
