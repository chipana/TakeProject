using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TakeProject.Server.Extensions;
using TakeProject.Server.Handlers.Chat;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Handlers
{
    public class WebSocketRequestHandler
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ChatCommandHandler _chatCommandHandler;
        private readonly ChatMessageHandler _chatMessageHandler;
        private readonly ChatRegistrationHandler _chatRegistrationHandler;

        public WebSocketRequestHandler(IConnectionManager connectionManager,
            ChatCommandHandler chatCommandHandler, 
            ChatMessageHandler chatMessageHandler, 
            ChatRegistrationHandler chatRegistrationHandler)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _chatCommandHandler = chatCommandHandler ?? throw new ArgumentNullException(nameof(chatCommandHandler));
            _chatMessageHandler = chatMessageHandler ?? throw new ArgumentNullException(nameof(chatMessageHandler));
            _chatRegistrationHandler = chatRegistrationHandler ?? throw new ArgumentNullException(nameof(chatRegistrationHandler));
        }

        /// <summary>                                                 
        /// Recieves the request
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public async Task RecieveRequest(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var nickname = _connectionManager.GetNickNameBySocket(socket);

            var rawMessage = buffer.GetBufferedMessage(result.Count);

            if (string.IsNullOrWhiteSpace(nickname))
                await _chatRegistrationHandler.Handle(socket, nickname, rawMessage);
            else if (rawMessage.StartsWith("/"))
                await _chatCommandHandler.Handle(socket, nickname, rawMessage);
            else
                await _chatMessageHandler.Handle(socket, nickname, rawMessage); 
        }
    }
}
