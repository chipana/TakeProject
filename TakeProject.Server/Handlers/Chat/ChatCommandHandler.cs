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
    public class ChatCommandHandler : SocketHandler, IHandler
    {
        public ChatCommandHandler(ConnectionManager connections) : base(connections) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public async Task Handle(WebSocket socket, string nickname, string rawMessage)
        {
            var messageSplitted = rawMessage.Split(' ');
            if (!IsValidCommand(messageSplitted[0].Replace("/", string.Empty), messageSplitted.Length))
            {
                await SendMessage(socket, ServerMessageConstants.GetMessage(ServerMessageConstants.COMMAND_INVALID, messageSplitted[0]));
            }

            switch (messageSplitted[0])
            {
                case "/p":
                    await SendPrivateMessageCommand(socket, nickname, rawMessage, messageSplitted);
                    break;
                case "/exit":
                    await ExecuteExitCommand(socket);
                    break;
                case "/changenickname":
                    await ChangeNickNameCommand(socket, nickname, messageSplitted);
                    break;
                case "/help":
                    await ShowHelpListCommand(socket);
                    break;
            }

        }

        private async Task SendPrivateMessageCommand(WebSocket socket, string nickname, string rawMessage, string[] command)
        {
            var targetUser = command[1];

            rawMessage = rawMessage.Replace(command[0], string.Empty).Replace(command[1], string.Empty).Trim();

            var targetSocket = _connections.GetSocketByNickName(targetUser);

            if (targetSocket != null)
                await SendPrivateMessage(socket, targetSocket, ServerMessageConstants.GetMessage(ServerMessageConstants.PRIVATE_MESSAGE, nickname, targetUser, rawMessage));
            else
                await SendMessage(socket, ServerMessageConstants.GetMessage(ServerMessageConstants.USER_NOT_FOUND, targetUser));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ShowHelpListCommand(WebSocket webSocket)
        {
            await webSocket.SendAsync(BufferHelper.GetMessageToSend(string.Join("\n", CommandHelper.ValidCommands.Select(p => p.Description))), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ChangeNickNameCommand(WebSocket webSocket, string nickname, string[] command)
        {
            _connections.RegisterNickName(webSocket, command[1], nickname);
            await SendMessage(webSocket, ServerMessageConstants.GetMessage(ServerMessageConstants.COMMAND_INVALID, command[1]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private async Task ExecuteExitCommand(WebSocket webSocket)
        {
            await OnDisconnected(webSocket);
        }


        /// <summary>
        /// 
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
