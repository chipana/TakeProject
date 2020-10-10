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
    public class ChatRegistrationHandler : SocketHandler, IHandler
    {
        public ChatRegistrationHandler(ConnectionManager connections) : base(connections) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public async Task Handle(WebSocket socket, string nickname, string rawMessage)
        {
            if (!IsValidNickName(rawMessage))
            {
                await SendMessage(socket, ServerMessageConstants.GetMessage(ServerMessageConstants.NICKNAME_INVALID, rawMessage));
                return;
            }

            if (!_connections.IsNickNameExists(rawMessage))
            {
                await SendMessage(socket, ServerMessageConstants.GetMessage(ServerMessageConstants.NICKNAME_ALREADY_TAKEN, rawMessage));
                return;
            }


            if (_connections.RegisterNickName(socket, rawMessage))
            {
                await SendMessage(socket, ServerMessageConstants.GetMessage(ServerMessageConstants.SUCCESSFULLY_REGISTERED, rawMessage));

                var message = ServerMessageConstants.GetMessage(ServerMessageConstants.JOINED_GENERAL_CHANNEL, rawMessage);
                await SendMessageToAll(message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        private bool IsValidNickName(string nickname)
        {
            // Verify if there's any special character
            Regex regexExpression = new Regex("^[a-zA-Z0-9]*$");

            return !string.IsNullOrWhiteSpace(nickname) && !regexExpression.IsMatch(nickname);
        }
    }
}
