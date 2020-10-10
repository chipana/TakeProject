using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Handlers.Chat
{
    public class ChatRegistrationHandler : IHandler
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ISocketHandler _socketHandler;
        public ChatRegistrationHandler(IConnectionManager connectionManager, ISocketHandler socketHandler)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _socketHandler = socketHandler ?? throw new ArgumentNullException(nameof(socketHandler));
        }


        /// <summary>
        /// Handle the client registration
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public async Task<string> Handle(WebSocket socket, string nickname, string rawMessage)
        {
            var result = "";
            if (!IsValidNickName(rawMessage))
            {
                result = ServerMessageConstants.GetMessage(ServerMessageConstants.NICKNAME_INVALID, rawMessage);
                await _socketHandler.SendMessage(socket, result);
                return result;
            }

            if (_connectionManager.IsNickNameExists(rawMessage))
            {
                result = ServerMessageConstants.GetMessage(ServerMessageConstants.NICKNAME_ALREADY_TAKEN, rawMessage);
                await _socketHandler.SendMessage(socket, result);
                return result;
            }


            if (_connectionManager.RegisterNickName(socket, rawMessage))
            {
                result = ServerMessageConstants.GetMessage(ServerMessageConstants.SUCCESSFULLY_REGISTERED, rawMessage);
                await _socketHandler.SendMessage(socket, result);

                var message = ServerMessageConstants.GetMessage(ServerMessageConstants.JOINED_GENERAL_CHANNEL, rawMessage);
                await _socketHandler.SendMessageToAll(message);

            }
            return result;
        }


        /// <summary>
        /// Check if nickname is valid.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        private bool IsValidNickName(string nickname)
        {
            // Verify if there's any special character
            Regex regexExpression = new Regex("^[a-zA-Z0-9]*$");

            return !string.IsNullOrWhiteSpace(nickname) && regexExpression.IsMatch(nickname);
        }
    }
}
