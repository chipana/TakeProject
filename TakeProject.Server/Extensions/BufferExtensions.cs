using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeProject.Server.Extensions
{
    public static class BufferExtensions
    {
        public static string GetBufferedMessage(this byte[] buffer, int bufferLength) => Encoding.UTF8.GetString(buffer, 0, bufferLength);

    }
}
