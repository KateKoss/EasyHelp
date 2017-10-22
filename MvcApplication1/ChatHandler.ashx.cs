using MvcApplication1.Contexts;
using MvcApplication1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private bool isMassegeSent = false;

        // Блокировка для обеспечения потокабезопасности
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public void ProcessRequest(HttpContext context)
        {
            //Если запрос является запросом веб сокета
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(WebSocketRequest);
                
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {//якщо користувач онлайн то відправляємо повідомлення, якщо ні, то запис у бд  і дістаємо з бд тільки коли буде онлайн
            // Получаем сокет клиента из контекста запроса
            var socket = context.WebSocket;
            //List<string> l = new List<string>();
            //string user6 = "user6";
            //l.Add(user6);
            //string user5 = "user5";
            //l.Add(user5);
            //from db

            //foreach (var item in l)
            //{
                //GenerateToken g = new GenerateToken();
                //var jsonObj = g.GenerateLocalAccessTokenResponse(item);

                //JsonSerializer serializer = new JsonSerializer();
                //UserToken u = (UserToken)serializer.Deserialize(new JTokenReader(jsonObj), typeof(UserToken));

                var cookie = HttpContext.Current.Request.Cookies["UserId"].Value;
                ChatClients chc = new ChatClients
                {
                    SocketClient = socket,
                    UserName = cookie
                };



                // Добавляем его в список клиентов
                Locker.EnterWriteLock();
                try
                {
                    //MessageWebSocket cl = new MessageWebSocket();
                    //cl.SetRequestHeader("Cookie", "CookieName" + "=" + "CookieValue");
                    //from user token
                    var clientIsExist = false;
                    foreach (var item in Clients)
                    {
                        if (item.UserName == chc.UserName)
                        {
                            clientIsExist = true;
                            break;
                        }

                    }
                    if (!clientIsExist)
                        Clients.Add(chc);
                    //Clients.Add(socket);

                }
                finally
                {
                    Locker.ExitWriteLock();
                }
            //}
            // Слушаем его
            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);

                // Ожидаем данные от него
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                

                byte[] payloadData = buffer.Array.Where(b => b != 0).ToArray();

                //Because we know that is a string, we convert it. 
                string receiveString =
                    System.Text.Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);
                dynamic json = System.Web.Helpers.Json.Decode(@receiveString);
                //Передаём сообщение всем клиентам
                for (int i = 0; i < Clients.Count; i++)
                { 
                    //конвертувати ід в токен 
                    // ыд взяти токен і порівняти
                    WebSocket client = Clients[i].SocketClient;

                    try
                    {
                        if (client.State == WebSocketState.Open)
                        {
                            

                            //GenerateToken g = new GenerateToken();
                            //var jsonObj = g.GenerateLocalAccessTokenResponse(json.ToUser);

                            //JsonSerializer serializer = new JsonSerializer();
                            //UserToken u = (UserToken)serializer.Deserialize(new JTokenReader(jsonObj), typeof(UserToken));

                            //var cookie = HttpContext.Current.Request.Cookies["UT"];
                            //if (cookie != null)
                            //{
                                if (Clients[i].UserName == json.ToUser || Clients[i].UserName == json.FromUser)
                                {

                                    //only for windows 8
                                    //JsonValue jsonValue = JsonValue.Parse("{\"Width\": 800, \"Height\": 600, \"Title\": \"View from 15th Floor\", \"IDs\": [116, 943, 234, 38793]}");
                                    //double width = jsonValue.GetObject().GetNamedNumber("Width");
                                    //double height = jsonValue.GetObject().GetNamedNumber("Height");
                                    //string title = jsonValue.GetObject().GetNamedString("Title");
                                    //JsonArray ids = jsonValue.GetObject().GetNamedArray("IDs");

                                    await client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                                    isMassegeSent = true;
                                }
                           // }
                        }

                        var toUser = json.ToUser;
                        var mess = json.Message;
                        using (CustomDbContext db = new CustomDbContext())
                        {
                            db.ChatMessages.Add(new ChatMessageModel
                            {

                                FromUser = cookie,
                                ToUser = toUser,
                                MessageText = mess,
                                DateTimeSent = DateTime.Now,
                                isMessageSent = isMassegeSent

                            });
                            db.SaveChanges();
                        }

                        if (!isMassegeSent)
                        {

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