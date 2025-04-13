using Devsquare.EasyCQRS.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Devsquare.EasyCQRS.Implementations;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds CQRS services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="assemblies">The assemblies to scan for command and query handlers</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddEasyCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        // Register dispatchers
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        
        // Register command handlers
        RegisterCommandHandlers(services, assemblies);
        
        // Register query handlers
        RegisterQueryHandlers(services, assemblies);
        
        return services;
    }
    
    private static void RegisterCommandHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        // Register handlers for commands without result
        var commandHandlerType = typeof(ICommandHandler<>);
        RegisterHandlers(services, commandHandlerType, assemblies);
        
        // Register handlers for commands with result
        var commandWithResultHandlerType = typeof(ICommandHandler<,>);
        RegisterHandlers(services, commandWithResultHandlerType, assemblies);
    }
    
    private static void RegisterQueryHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var queryHandlerType = typeof(IQueryHandler<,>);
        RegisterHandlers(services, queryHandlerType, assemblies);
    }
    
    private static void RegisterHandlers(IServiceCollection services, Type handlerType, Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            // Find all types that implement the handler interface
            var handlers = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType));

            foreach (var handler in handlers)
            {
                // Get all interfaces that match the handler type
                var interfaces = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);

                foreach (var handlerInterface in interfaces)
                {
                    services.AddScoped(handlerInterface, handler);
                }
            }
        }
    }
}
