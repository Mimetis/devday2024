namespace TaskCompletionSourceExample
{
    public class WebAuthenticationAsync
    {

        public WebAuthenticationAsync() { }


        public async Task<string> AuthenticateAsync(string username, string password)
        {
            // By default, Task continuations will run inline on the same thread that calls Try/Set(Result/Exception/Canceled).
            // As a library author, this means having to understand that calling code can resume directly on your thread.
            // This is extremely dangerous and can result in deadlocks, thread-pool starvation,
            // corruption of state (if code runs unexpectedly) and more
            //Console.WriteLine($"AuthenticateAsync. Call. ManagedThreadId: {Environment.CurrentManagedThreadId}");

            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            var webLegacyAuthentication = new WebLegacyAuthentication();
            webLegacyAuthentication.OnAuthenticationSuccess += (token) =>
            {
                //Console.WriteLine($"OnAuthenticationSuccess. Before SetResult. ManagedThreadId: {Environment.CurrentManagedThreadId}");
                tcs.SetResult(token);
                //Console.WriteLine($"OnAuthenticationSuccess. After SetResult. ManagedThreadId: {Environment.CurrentManagedThreadId}");
            };
            webLegacyAuthentication.OnAuthenticationFailure += (ex) => tcs.SetException(ex);

            webLegacyAuthentication.Authenticate(username, password);
            var result = await tcs.Task;
            //Console.WriteLine($"AuthenticateAsync. After await. ManagedThreadId: {Environment.CurrentManagedThreadId}");

            return result;
        }
    }
}
