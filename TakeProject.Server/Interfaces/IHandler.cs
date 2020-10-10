using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TakeProject.Server.Interfaces
{
    public interface IHandler
    {
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nickname"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        Task<string> Handle(WebSocket socket, string nickname, string rawMessage);
    }
}
