using Moq;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Handlers.Chat;
using TakeProject.Server.Helpers;
using TakeProject.Server.Interfaces;
using Xunit;

namespace TakeProject.Tests
{
    public class ServerRegistrationTests
    {
        [Fact]
        public async Task GivenSocketRegistrationWhenExecutingHandleThanReturnTheRegistrationMessage()
        {
            var emptyNickName = "";
            var newNickName = "foo";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.SUCCESSFULLY_REGISTERED, newNickName);

            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            connectionManagerMock.Setup(p => p.RegisterNickName(It.IsAny<WebSocket>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatRegistrationHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, emptyNickName, newNickName);

            Assert.Equal(taskResult, expected);
        }

        [Fact]
        public async Task GivenInvalidNickNameWhenExecutingHandleThanReturnTheInvalidNickNameErrorMessage()
        {
            var emptyNickName = "";
            var newNickName = "$foo$";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.NICKNAME_INVALID, newNickName);

            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            connectionManagerMock.Setup(p => p.RegisterNickName(It.IsAny<WebSocket>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatRegistrationHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, emptyNickName, newNickName);

            Assert.Equal(taskResult, expected);
        }
        [Fact]
        public async Task GivenAnExistingNickNameWhenExecutingHandleThanReturnTheNickNameAlreadyTakenErrorMessage()
        {
            var emptyNickName = "";
            var newNickName = "foo";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.NICKNAME_ALREADY_TAKEN, newNickName);

            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<IConnectionManager> connectionManagerMock = new Mock<IConnectionManager>();
            connectionManagerMock.Setup(p => p.IsNickNameExists(It.IsAny<string>())).Returns(true);
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();
            var chatCommandHandler = new ChatRegistrationHandler(connectionManagerMock.Object, socketHandlerMock.Object);

            var taskResult = await chatCommandHandler.Handle(originSocketMock.Object, emptyNickName, newNickName);

            Assert.Equal(taskResult, expected);
        }
    }
}
