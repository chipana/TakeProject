using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakeProject
{
    class Program
    {
        private static ClientWebSocket client;
        static void Main(string[] args)
        {
            StartWebSockets().GetAwaiter().GetResult();

            //var userName = Console.ReadLine();
            //var invalidUserName = userName.Equals("ble");
            //while(invalidUserName)
            //{
            //    Console.WriteLine($"*** Sorry, the nickname {userName} is already taken. Please choose a different one: ");
            //    userName = Console.ReadLine();
            //    invalidUserName = userName.Equals("ble");
            //}

            //Console.WriteLine($"*** You are registered as {userName}. Joining #general");
            //Console.WriteLine("*** Disconected. Bye!");
            //Console.ReadKey();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (client.State == WebSocketState.Open)
                client.Dispose();
        }

        private static async Task StartWebSockets()
        {
            client = new ClientWebSocket();
            await client.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);

            var send = Task.Run(async () =>
            {
                string message;
                while ((message = Console.ReadLine()) != null && message != string.Empty)
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            });

            var recieve = RecieveAsync(client);
            await Task.WhenAll(send, recieve);
        }

        public static async Task RecieveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var resut = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, resut.Count));
                if (resut.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }
        }
    }
}
