using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TakeProject.Server.Handlers;
using TakeProject.Server.Handlers.Chat;
using TakeProject.Server.Interfaces;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Declare components in Dependency Injection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddSingleton<ISocketHandler, SocketHandler>();
            services.AddSingleton<WebSocketRequestHandler>();
            services.AddSingleton<ChatCommandHandler>();
            services.AddSingleton<ChatMessageHandler>();
            services.AddSingleton<ChatRegistrationHandler>();
            return services;
        }
    }
}
