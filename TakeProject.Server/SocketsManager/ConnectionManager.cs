using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakeProject.Server.Interfaces;

namespace TakeProject.Server.SocketsManager
{
    public class ConnectionManager : IConnectionManager
    {
        private ConcurrentDictionary<WebSocket, string> _connections = new ConcurrentDictionary<WebSocket, string>();

        /// <summary>
        /// List of connections
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<WebSocket, string> GetAllConnections() => _connections;

        /// <summary>
        /// Try to add a connection
        /// </summary>
        /// <param name="socket"></param>
        public void AddConnection(WebSocket socket) => _connections.TryAdd(socket, "");

        /// <summary>
        /// Try to remove a connection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveConnectionAsync(string id)
        {
            var socket = GetSocketByNickName(id);
            _connections.TryRemove(socket, out var nickname);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Take Socket Closed", CancellationToken.None);
        }

        /// <summary>
        /// Gets the socket by client nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public WebSocket GetSocketByNickName(string nickname) => _connections.FirstOrDefault(con => string.Equals(con.Value, nickname, StringComparison.OrdinalIgnoreCase)).Key;
        
        /// <summary>
        /// Get the connection by client nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public KeyValuePair<WebSocket, string> GetConnectionByNickName(string nickname) => _connections.FirstOrDefault(con => string.Equals(con.Value, nickname, StringComparison.OrdinalIgnoreCase));
        
        /// <summary>
        /// Get the nickname by client socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public string GetNickNameBySocket(WebSocket socket) => _connections.FirstOrDefault(con => con.Key == socket).Value;

        /// <summary>
        /// Try to register the client nickname to websocket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="newNickName"></param>
        /// <param name="oldNickName"></param>
        /// <returns></returns>
        public bool RegisterNickName(WebSocket socket, string newNickName, string oldNickName = "") => _connections.TryUpdate(socket, newNickName, oldNickName);

        /// <summary>
        /// Check if nickname already exists in connection list
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool IsNickNameExists(string nickname) => _connections.Any(c => string.Equals(c.Value, nickname, StringComparison.OrdinalIgnoreCase));
    }
}
