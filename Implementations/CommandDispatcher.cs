using Devsquare.EasyCQRS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Devsquare.EasyCQRS.Implementations;

/// <summary>
/// Implementation of the command dispatcher
/// </summary>
public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    /// <inheritdoc />
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : ICommand
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : ICommand<TResult>
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        // Get the actual command type
        var commandType = command.GetType();
        
        // Create the generic handler type
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));
        
        // Get the handler instance
        var handler = serviceProvider.GetRequiredService(handlerType);
        
        // Get the HandleAsync method
        var method = handlerType.GetMethod("HandleAsync");
        
        // Invoke the method
        var task = (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken });
        
        // Await the task
        return await task;
    }
}
