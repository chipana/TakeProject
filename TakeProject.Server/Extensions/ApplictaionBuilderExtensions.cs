using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TakeProject.Server.Handlers;
using TakeProject.Server.Middlewares;

namespace TakeProject.Server.Extensions
{
    public static class ApplictaionBuilderExtensions
    {
        /// <summary>
        /// Injects the SocketMiddleware as a middleware, when path /ws was called
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path)
        {
            return app.Map(path, (x) =>
            {
                x.UseMiddleware<SocketMiddleware>();
            });
        }
    }
}
