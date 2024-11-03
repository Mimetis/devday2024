namespace TaskCompletionSourceExample
{
    internal class Program
    {
        //private static void Main(string[] args)
        //{
        //    var webLegacyAuthentication = new WebLegacyAuthentication();
        //    webLegacyAuthentication.OnAuthenticationSuccess += OnAuthenticationSuccess;
        //    webLegacyAuthentication.OnAuthenticationFailure += OnAuthenticationFailure;

        //    webLegacyAuthentication.Authenticate("admin", "admin");

        //    Console.WriteLine("End");

        //    Console.ReadLine();
        //}


        public static void OnAuthenticationSuccess(string message)
        {
            Console.WriteLine(message);
        }

        public static void OnAuthenticationFailure(Exception ex)
        {
            Console.WriteLine(ex);
        }

        //public static async Task Main(string[] args)
        //{
        //    var webLegacyAuthentication = new WebLegacyAuthentication();

        //    var tcs = new TaskCompletionSource<string>();

        //    webLegacyAuthentication.OnAuthenticationSuccess += (message) => tcs.SetResult(message);
        //    webLegacyAuthentication.OnAuthenticationFailure += (ex) => tcs.SetException(ex);

        //    webLegacyAuthentication.Authenticate("admin", "admin");
        //    try
        //    {
        //        var result = await tcs.Task;
        //        Console.WriteLine(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    Console.WriteLine("End");
        //    Console.ReadLine();

        //}


        private static async Task Main(string[] args)
        {
            try
            {
                //Console.WriteLine($"Main. Start. ManagedThreadId: {Environment.CurrentManagedThreadId}");

                var webLegacyAuthentication = new WebAuthenticationAsync();
                var token = await webLegacyAuthentication.AuthenticateAsync("admin", "admin");
                Console.WriteLine(token);
                //Console.WriteLine($"Main. End. ManagedThreadId: {Environment.CurrentManagedThreadId}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("End");

            Console.ReadLine();
        }

    }
}
