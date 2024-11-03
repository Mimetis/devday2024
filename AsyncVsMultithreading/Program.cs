namespace AsyncVsMultithreading
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // – Multithreading is a programming technique for executing operations running on multiple threads(also called workers)
            // where we use different threads and block them until the job is done.
            // - Asynchronous programming is the concurrent execution of multiple tasks
            // (here the assigned thread is returned back to a thread pool once the await keyword is reached in the method).


            await ExecuteAsyncFunctions();

            ExecuteMultithreading();
        }

        public static async Task ExecuteAsyncFunctions()
        {
            var firstAsync = FirstAsync();
            var secondAsync = SecondAsync();
            var thirdAsync = ThirdAsync();
            await Task.WhenAll(firstAsync, secondAsync, thirdAsync);
        }


        public static void ExecuteMultithreading()
        {
            Thread t1 = new(new ThreadStart(FirstMethod));
            Thread t2 = new(new ThreadStart(SecondMethod));
            Thread t3 = new(new ThreadStart(ThirdMethod));
            t1.Start();
            t2.Start();
            t3.Start();
        }

        public static async Task FirstAsync()
        {
            Console.WriteLine("First Async Method on Thread with Id: " + Environment.CurrentManagedThreadId);
            await Task.Delay(1000);
            Console.WriteLine("First Async Method Continuation on Thread with Id: " + Environment.CurrentManagedThreadId);
        }

        public static async Task SecondAsync()
        {
            Console.WriteLine("Second Async Method on Thread with Id: " + Environment.CurrentManagedThreadId);
            await Task.Delay(1000);
            Console.WriteLine("Second Async Method Continuation on Thread with Id: " + Environment.CurrentManagedThreadId);
        }

        public static async Task ThirdAsync()
        {
            Console.WriteLine("Third Async Method on Thread with Id: " + Environment.CurrentManagedThreadId);
            await Task.Delay(1000);
            Console.WriteLine("Third Async Method Continuation on Thread with Id: " + Environment.CurrentManagedThreadId);
        }


        public static void FirstMethod()
        {
            Console.WriteLine("First Method on Thread with Id: " + Environment.CurrentManagedThreadId);
            Thread.Sleep(1000);
            Console.WriteLine("First Method Continuation on Thread with Id: " + Environment.CurrentManagedThreadId);
        }

        public static void SecondMethod()
        {
            Console.WriteLine("Second Method on Thread with Id: " + Environment.CurrentManagedThreadId);
            Thread.Sleep(1000);
            Console.WriteLine("Second Method Continuation on Thread with Id: " + Environment.CurrentManagedThreadId);
        }

        public static void ThirdMethod()
        {
            Console.WriteLine("Third Method on Thread with Id: " + Environment.CurrentManagedThreadId);
            Thread.Sleep(1000);
            Console.WriteLine("Third Method Continuation on Thread with Id: " + Environment.CurrentManagedThreadId);
        }
    }
}
