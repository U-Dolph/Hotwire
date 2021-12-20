using Hotwire.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hotwire
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static HttpService httpService;
        static WebSocketService webSocketService;

        public App()
        {
            InitializeComponent();
        }

        public static HttpService HttpService
        {
            get
            {
                if (httpService == null)
                    httpService = new HttpService();
                return httpService;
            }
        }

        public static WebSocketService WebSocketService
        {
            get
            {
                if (webSocketService == null)
                    webSocketService = new WebSocketService();
                return webSocketService;
            }
        }
    }
}
