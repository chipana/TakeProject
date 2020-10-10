using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TakeProject.Server.Interfaces
{
    public interface ISocketHandler
    {
        /// <summary>
        /// Add a connection to the list of connected clients and send a message requesting the nickname.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        Task OnConnected(WebSocket socket);

        /// <summary>
        /// Remove a connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        Task OnDisconnected(WebSocket socket);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        Task RecieveRequest(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

        /// <summary>
        /// Sends a message to client.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(WebSocket socket, string message);

        /// <summary>
        /// Sends a message to client.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(string id, string message);

        /// <summary>
        /// Sends a message to origin and target client.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageToOriginTargetClients(WebSocket origin, WebSocket target, string message);

        /// <summary>
        /// Sends message to all clients.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageToAll(string message);
    }
}
