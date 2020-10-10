using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Helpers;
using TakeProject.Server.Interfaces;
using TakeProject.Server.Model;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Handlers.Chat
{
    public class ChatCommandHandler : IHandler
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ISocketHandler _socketHandler;
        public ChatCommandHandler(IConnectionManager connectionManager, ISocketHandler socketHandler)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _socketHandler = socketHandler ?? throw new ArgumentNullException(nameof(socketHandler));
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public async Task<string> Handle(WebSocket socket, string nickname, string rawMessage)
        {
            var result = "";
            var messageSplitted = rawMessage.Split(' ');
            if (!IsValidCommand(messageSplitted[0].Replace("/", string.Empty), messageSplitted.Length))
            {
                result = ServerMessageConstants.GetMessage(ServerMessageConstants.COMMAND_INVALID, messageSplitted[0]);
                await _socketHandler.SendMessage(socket, result);
            }

            switch (messageSplitted[0])
            {
                case "/p":
                    result = await SendPrivateMessageCommand(socket, nickname, rawMessage, messageSplitted);
                    break;
                case "/exit":
                    result = await ExecuteExitCommand(socket);
                    break;
                case "/changenickname":
                    result = await ChangeNickNameCommand(socket, nickname, messageSplitted);
                    break;
                case "/help":
                    result = await ShowHelpListCommand(socket);
                    break;
            }
            return result;

        }

        /// <summary>
        /// Sends a private message
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task<string> SendPrivateMessageCommand(WebSocket socket, string nickname, string rawMessage, string[] command)
        {
            var result = "";
            var targetUser = command[1];

            rawMessage = rawMessage.Replace(command[0], string.Empty).Replace(command[1], string.Empty).Trim();

            var targetSocket = _connectionManager.GetSocketByNickName(targetUser);

            if (targetSocket != null)
            {
                result = ServerMessageConstants.GetMessage(ServerMessageConstants.PRIVATE_MESSAGE, nickname, targetUser, rawMessage);
                await _socketHandler.SendMessageToOriginTargetClients(socket, targetSocket, result);
            }
            else
            {
                result = ServerMessageConstants.GetMessage(ServerMessageConstants.USER_NOT_FOUND, targetUser);
                await _socketHandler.SendMessage(socket, result);
            }
            return result;
        }

        /// <summary>
        /// Sends a list of commands to client.
        /// </summary>
        /// <returns></returns>
        private async Task<string> ShowHelpListCommand(WebSocket webSocket)
        {
            var result = string.Join("\n", CommandHelper.ValidCommands.Select(p => p.Description));
            await webSocket.SendAsync(BufferHelper.GetMessageToSend(result), WebSocketMessageType.Text, true, CancellationToken.None);
            return result;
        }

        /// <summary>
        /// Change client nickname
        /// </summary>
        /// <returns></returns>
        private async Task<string> ChangeNickNameCommand(WebSocket webSocket, string nickname, string[] command)
        {
            _connectionManager.RegisterNickName(webSocket, command[1], nickname);
            var result = ServerMessageConstants.GetMessage(ServerMessageConstants.SUCCESSFULLY_CHANGED_NICKNAME, command[1]);
            await _socketHandler.SendMessage(webSocket, result);
            return result;
        }

        /// <summary>
        /// Executes the exi command
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private async Task<string> ExecuteExitCommand(WebSocket webSocket)
        {
            await _socketHandler.OnDisconnected(webSocket);
            return ServerMessageConstants.DISCONNECT_MESSAGE;
        }


        /// <summary>
        /// Check if command is valid.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private bool IsValidCommand(string command, int parameters)
        {
            var validCommand = CommandHelper.ValidCommands.SingleOrDefault(p => p.CommandText == command);
            return validCommand != null && parameters >= validCommand.Parameters;
        }
    }
}
