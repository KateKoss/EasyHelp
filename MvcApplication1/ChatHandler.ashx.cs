using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace MvcApplication1
{
    /// <summary>
    /// Summary description for ChatHandler
    /// </summary>
    public class ChatHandler : IHttpHandler
    {
        // Список всех клиентов
        private static readonly List<ChatClients> Clients = new List<ChatClients>();

        // Блокировка для обеспечения потокабезопасности
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public void ProcessRequest(HttpContext context)
        {
            //Если запрос является запросом веб сокета
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(WebSocketRequest);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {
            // Получаем сокет клиента из контекста запроса
            var socket = context.WebSocket;
            string token = "rfkd";
            ChatClients chc = new ChatClients
            {
                SocketClient = socket,
                userName = token
            };

            // Добавляем его в список клиентов
            Locker.EnterWriteLock();
            try
            {
                //MessageWebSocket cl = new MessageWebSocket();
                //cl.SetRequestHeader("Cookie", "CookieName" + "=" + "CookieValue");
                //from user token
                Clients.Add(chc);
            }
            finally
            {
                Locker.ExitWriteLock();
            }

            // Слушаем его
            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);

                // Ожидаем данные от него
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                
                //Передаём сообщение всем клиентам
                for (int i = 0; i < Clients.Count; i++)
                {
                    GenerateToken g = new GenerateToken();
                    g.GenerateLocalAccessTokenResponse(Clients[i].userName);
                    if (Clients[i].userName == "skd")
                    {


                        //конвертувати ід в токен 
                        // ыд взяти токен і порівняти
                        WebSocket client = Clients[i].SocketClient;

                        try
                        {
                            if (client.State == WebSocketState.Open)
                            {
                                byte[] payloadData = buffer.Array.Where(b => b != 0).ToArray();

                                //Because we know that is a string, we convert it. 
                                string receiveString =
                                  System.Text.Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);
                                dynamic json = System.Web.Helpers.Json.Decode(@receiveString);
                                client = json.ToUser;


                                //only for windows 8
                                //JsonValue jsonValue = JsonValue.Parse("{\"Width\": 800, \"Height\": 600, \"Title\": \"View from 15th Floor\", \"IDs\": [116, 943, 234, 38793]}");
                                //double width = jsonValue.GetObject().GetNamedNumber("Width");
                                //double height = jsonValue.GetObject().GetNamedNumber("Height");
                                //string title = jsonValue.GetObject().GetNamedString("Title");
                                //JsonArray ids = jsonValue.GetObject().GetNamedArray("IDs");

                                await client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }

                        catch (ObjectDisposedException)
                        {
                            Locker.EnterWriteLock();
                            try
                            {
                                Clients.RemoveAll(x => x.SocketClient == socket);
                                i--;
                            }
                            finally
                            {
                                Locker.ExitWriteLock();
                            }
                        }
                    }
                }

            }
        }
    }

    
}