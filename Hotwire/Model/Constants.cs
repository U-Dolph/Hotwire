using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class Constants
    {
        //private static string serverUrl = "http://dolphvault.duckdns.org:1234/";
        private static string serverUrl = "http://localhost:1234/";

        public static string LoginUrl { get => serverUrl + "login"; }
        public static string RegisterUrl { get => serverUrl + "register"; }
        public static string SocketUrl { get => serverUrl + "request_socket"; }
    }
}
