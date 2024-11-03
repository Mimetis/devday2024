namespace TaskCompletionSourceExample
{
    public class WebLegacyAuthentication
    {
        public event Action<Exception> OnAuthenticationFailure;
        public event Action<string> OnAuthenticationSuccess;

        public void Authenticate(string username, string password)
        {
            // simulate a web request
            new Thread(() =>
            {
                Thread.Sleep(1000);
                if (username == "admin" && password == "admin")
                {
                    OnAuthenticationSuccess?.Invoke("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
                }
                else
                {
                    OnAuthenticationFailure?.Invoke(new Exception("Invalid_credentials"));
                }
            }).Start();
        }
    }
}