using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Web;

namespace MvcApplication1
{
    public class ChatClients
    {
        public WebSocket SocketClient { get; set; }
        public string UserName { get; set; }
    }
}