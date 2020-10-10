using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TakeProject.Server.Interfaces
{
    public interface IConnectionManager
    {
        /// <summary>
        /// List of connections
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<WebSocket, string> GetAllConnections();

        /// <summary>
        /// Try to add a connection
        /// </summary>
        /// <param name="socket"></param>
        void AddConnection(WebSocket socket);

        /// <summary>
        /// Try to remove a connection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveConnectionAsync(string id);

        /// <summary>
        /// Gets the socket by client nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        WebSocket GetSocketByNickName(string nickname);

        /// <summary>
        /// Get the connection by client nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        KeyValuePair<WebSocket, string> GetConnectionByNickName(string nickname);

        /// <summary>
        /// Get the nickname by client socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        string GetNickNameBySocket(WebSocket socket);

        /// <summary>
        /// Try to register the client nickname to websocket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="newNickName"></param>
        /// <param name="oldNickName"></param>
        /// <returns></returns>
        bool RegisterNickName(WebSocket socket, string newNickName, string oldNickName = "");

        /// <summary>
        /// Check if nickname already exists in connection list
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        bool IsNickNameExists(string nickname);
    }
}
