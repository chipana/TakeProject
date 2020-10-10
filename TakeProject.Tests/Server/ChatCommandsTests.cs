using Moq;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Handlers;
using TakeProject.Server.Handlers.Chat;
using TakeProject.Server.Helpers;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;
using Xunit;

namespace TakeProject.Tests
{
    public class ChatCommandsTests
    {
        [Fact]
        public async Task GivenPrivateMessageCommandWhenExecutingHandleThanReturnTheCommandExecutionMessage()
        {
            var nickname = "foo";
            var command = "/p bar message";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.PRIVATE_MESSAGE, nickname, "bar", "message");
            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<WebSocket> targetSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            connectionManagerMock.Setup(p => p.GetSocketByNickName(It.IsAny<string>())).Returns(targetSocketMock.Object);
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatCommandHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, nickname, command);

            Assert.Equal(taskResult, expected);
        }

        [Fact]
        public async Task GivenExitCommandWhenExecutingHandleThanReturnTheCommandExecutionMessage()
        {
            var nickname = "foo";
            var command = "/exit";
            var expected = ServerMessageConstants.DISCONNECT_MESSAGE;
            Mock<WebSocket> webSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatCommandHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(webSocketMock.Object, nickname, command);

            Assert.Equal(taskResult, expected);
        }

        [Fact]
        public async Task GivenChangeNickNameCommandWhenExecutingHandleThanReturnTheCommandExecutionMessage()
        {
            var nickname = "foo";
            var command = "/changenickname bar";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.SUCCESSFULLY_CHANGED_NICKNAME, "bar");
            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatCommandHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, nickname, command);

            Assert.Equal(taskResult, expected);
        }

        [Fact]
        public async Task GivenHelpCommandWhenExecutingHandleThanReturnTheCommandExecutionMessage()
        {
            var nickname = "foo";
            var command = "/help";
            var expected = string.Join("\n", CommandHelper.ValidCommands.Select(p => p.Description));

            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatCommandHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, nickname, command);

            Assert.Equal(taskResult, expected);
        }

        [Fact]
        public async Task GivenInvalidCommandWhenExecutingHandleThanReturnTheInvalidCommandErrorMessage()
        {
            var nickname = "foo";
            var command = "/barbar";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.COMMAND_INVALID, command);

            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatCommandHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, nickname, command);

            Assert.Equal(taskResult, expected);
        }
    }
}
