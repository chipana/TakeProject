using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeProject.Server
{
    public static class BufferHelper
    {
        /// <summary>
        /// Gets the ArraySegment from message string.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ArraySegment<byte> GetMessageToSend(string message) => new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length);
    }
}
