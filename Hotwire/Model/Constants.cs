using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class Constants
    {
        //public static string ServerUrl { get => "http://dolphvault.duckdns.org:1234/"; }
        public static string ServerUrl { get => "http://localhost:1234/"; private set { } }

        public static string LoginUrl { get => ServerUrl + "login"; }
        public static string RegisterUrl { get => ServerUrl + "register"; }
        public static string SocketUrl { get => ServerUrl + "request_socket"; }
    }
}
