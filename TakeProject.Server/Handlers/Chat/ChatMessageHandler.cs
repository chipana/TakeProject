using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Handlers.Chat
{
    public class ChatMessageHandler : IHandler
    {
        private readonly ISocketHandler _socketHandler;
        public ChatMessageHandler(ISocketHandler socketHandler)
        {
            _socketHandler = socketHandler ?? throw new ArgumentNullException(nameof(socketHandler));
        }

        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public async Task<string> Handle(WebSocket socket, string nickname, string rawMessage)
        {
            var message = ServerMessageConstants.GetMessage(ServerMessageConstants.GENERAL_MESSAGE, nickname, rawMessage);

            await _socketHandler.SendMessageToAll(message);

            return message;
        }
    }
}
