using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeProject.Server.Extensions
{
    public static class BufferExtensions
    {
        /// <summary>
        /// Gets the buffered string from byte array.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferLength"></param>
        /// <returns></returns>
        public static string GetBufferedMessage(this byte[] buffer, int bufferLength) => Encoding.UTF8.GetString(buffer, 0, bufferLength);

    }
}
