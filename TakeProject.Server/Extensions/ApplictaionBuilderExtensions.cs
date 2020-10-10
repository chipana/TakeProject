using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TakeProject.Server.Handlers;
using TakeProject.Server.Middlewares;

namespace TakeProject.Server.Extensions
{
    public static class ApplictaionBuilderExtensions
    {
        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path, SocketHandler socket)
        {
            return app.Map(path, (x) =>
            {
                x.UseMiddleware<SocketMiddleware>(socket);
            });
        }
    }
}
