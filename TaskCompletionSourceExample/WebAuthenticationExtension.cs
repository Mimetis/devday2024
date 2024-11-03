//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TaskCompletionSourceExample
//{
//    public static class WebAuthenticationExtension
//    {
//        public static Task<string> AuthenticateAsync(this WebLegacyAuthentication webLegacyAuthentication, string username, string password)
//        {
//            return Task<string>.Factory.FromAsync(webLegacyAuthentication.BeginAuthenticate,
//                                               webLegacyAuthentication.EndAuthenticate, username, password, null);
//        }
//    }
//}
