namespace Devsquare.EasyCQRS.Interfaces;

/// <summary>
/// Interface for dispatching commands
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Dispatches a command that doesn't return a result
    /// </summary>
    /// <typeparam name="TCommand">The type of command to dispatch</typeparam>
    /// <param name="command">The command to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : ICommand;

    /// <summary>
    /// Dispatches a command that returns a result
    /// </summary>
    /// <typeparam name="TCommand">The type of command to dispatch</typeparam>
    /// <typeparam name="TResult">The type of result returned by the command</typeparam>
    /// <param name="command">The command to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the command</returns>
    Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : ICommand<TResult>;
        
    /// <summary>
    /// Dispatches a command that returns a result, inferring the result type from the command
    /// </summary>
    /// <typeparam name="TCommand">The type of command to dispatch</typeparam>
    /// <param name="command">The command to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the command</returns>
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
