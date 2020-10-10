using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TakeProject.Server.Interfaces
{
    public interface IHandler
    {
        Task Handle(WebSocket socket, string nickname, string rawMessage);
    }
}
