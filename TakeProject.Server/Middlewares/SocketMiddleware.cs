using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TakeProject.Server.Handlers;

namespace TakeProject.Server.Middlewares
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        private SocketHandler _socketHandler { get; set; }

        public SocketMiddleware(RequestDelegate next, SocketHandler socketHandler)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _socketHandler = socketHandler ?? throw new ArgumentNullException(nameof(socketHandler));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await _socketHandler.OnConnected(socket);

            await Recieve(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                    await _socketHandler.RecieveRequest(socket, result, buffer);
                else if (result.MessageType == WebSocketMessageType.Close)
                    await _socketHandler.OnDisconnected(socket);
            });
        }
        private async Task Recieve(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageHandler)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandler(result, buffer);
            }
        }
    }
}