using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TakeProject.Server.Extensions;
using TakeProject.Server.Handlers.Chat;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Handlers
{
    public class WebSocketRequestHandler : SocketHandler
    {
        private ChatCommandHandler _chatCommandHandler;
        private ChatMessageHandler _chatMessageHandler;
        private ChatRegistrationHandler _chatRegistrationHandler;

        public WebSocketRequestHandler(IConnectionManager connections, 
            ChatCommandHandler chatCommandHandler, 
            ChatMessageHandler chatMessageHandler, 
            ChatRegistrationHandler chatRegistrationHandler) : base(connections) 
        {
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
        public override async Task RecieveRequest(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var nickname = _connections.GetNickNameBySocket(socket);

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
