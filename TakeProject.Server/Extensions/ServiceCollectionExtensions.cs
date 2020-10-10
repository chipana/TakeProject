using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TakeProject.Server.Handlers;
using TakeProject.Server.SocketsManager;

namespace TakeProject.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandler))
                    services.AddSingleton(type);
            }
            return services;
        }
    }
}
