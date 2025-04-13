using Devsquare.EasyCQRS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Devsquare.EasyCQRS.Implementations;

/// <summary>
/// Implementation of the query dispatcher
/// </summary>
public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    /// <inheritdoc />
    public async Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : IQuery<TResult>
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        // Get the actual query type
        var queryType = query.GetType();
        
        // Create the generic handler type
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
        
        // Get the handler instance
        var handler = serviceProvider.GetRequiredService(handlerType);
        
        // Get the HandleAsync method
        var method = handlerType.GetMethod("HandleAsync");
        
        // Invoke the method
        var task = (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken });
        
        // Await the task
        return await task;
    }
}
