using Moq;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TakeProject.Server.Constants;
using TakeProject.Server.Handlers.Chat;
using TakeProject.Server.Interfaces;
using Xunit;

namespace TakeProject.Tests
{
    public class ServerMessageTests
    {
        [Fact]
        public async Task GivenAMessageToAllWhenExecutingHandleThanReturnTheFormattedMessage()
        {
            var nickname = "foo";
            var message = "message";
            var expected = ServerMessageConstants.GetMessage(ServerMessageConstants.GENERAL_MESSAGE, nickname, message);

            Mock<WebSocket> originSocketMock = new Mock<WebSocket>();
            Mock<ISocketHandler> socketHandlerMock = new Mock<ISocketHandler>();

            var chatMessageHandler = new ChatMessageHandler(socketHandlerMock.Object);

            var taskResult = await chatMessageHandler.Handle(originSocketMock.Object, nickname, message);

            Assert.Equal(taskResult, expected);
        }
    }
}
