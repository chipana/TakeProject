using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakeProject.Server.SocketsManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<WebSocket, string> _connections = new ConcurrentDictionary<WebSocket, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<WebSocket, string> GetAllConnections() => _connections;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public void AddConnection(WebSocket socket) => _connections.TryAdd(socket, "");

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public WebSocket GetSocketByNickName(string nickname) => _connections.FirstOrDefault(con => string.Equals(con.Value, nickname, StringComparison.OrdinalIgnoreCase)).Key;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public KeyValuePair<WebSocket, string> GetConnectionByNickName(string nickname) => _connections.FirstOrDefault(con => string.Equals(con.Value, nickname, StringComparison.OrdinalIgnoreCase));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public string GetNickNameBySocket(WebSocket socket) => _connections.FirstOrDefault(con => con.Key == socket).Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="newNickName"></param>
        /// <param name="oldNickName"></param>
        /// <returns></returns>
        public bool RegisterNickName(WebSocket socket, string newNickName, string oldNickName = "")
        {
            return !_connections.Any(c => _connections.TryUpdate(socket, newNickName, oldNickName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool IsNickNameExists(string nickname) => _connections.Any(c => string.Equals(c.Value, nickname, StringComparison.OrdinalIgnoreCase));
    }
}
