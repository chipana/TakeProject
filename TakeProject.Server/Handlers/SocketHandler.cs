using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Handlers
{
    public abstract class SocketHandler : ISocketHandler
    {
        protected IConnectionManager _connections { get; set; }
        public SocketHandler(IConnectionManager connection)
        {
            _connections = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Add a connection to the list of connected clients and send a message requesting the nickname.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => 
            {
                _connections.AddConnection(socket); 
            });

            await socket.SendAsync(BufferHelper.GetMessageToSend(ServerMessageConstants.PROVIDE_NICKNAME), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Remove a connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public async Task OnDisconnected(WebSocket socket)
        {
            await socket.SendAsync(BufferHelper.GetMessageToSend(ServerMessageConstants.DISCONNECT_MESSAGE), WebSocketMessageType.Text, true, CancellationToken.None);
            await _connections.RemoveConnectionAsync(_connections.GetNickNameBySocket(socket));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public virtual Task RecieveRequest(WebSocket socket, WebSocketReceiveResult result, byte[] buffer) 
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends a message to client.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            await socket.SendAsync(BufferHelper.GetMessageToSend(message), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Sends a message to client.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string id, string message)
        {
            await SendMessage(_connections.GetSocketByNickName(id), message);
        }

        /// <summary>
        /// Sends a message to origin and target client.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToOriginTargetClients(WebSocket origin, WebSocket target, string message)
        {
            await SendMessage(origin, message);
            await SendMessage(target, message);
        }

        /// <summary>
        /// Sends message to all clients.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAll(string message)
        {
            foreach (var con in _connections.GetAllConnections())
                await SendMessage(con.Value, message);
        }
    }
}
