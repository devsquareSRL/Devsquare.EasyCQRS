namespace Devsquare.EasyCQRS.Interfaces;

/// <summary>
/// Interface for dispatching queries
/// </summary>
public interface IQueryDispatcher
{
    /// <summary>
    /// Dispatches a query and returns the result
    /// </summary>
    /// <typeparam name="TQuery">The type of query to dispatch</typeparam>
    /// <typeparam name="TResult">The type of result returned by the query</typeparam>
    /// <param name="query">The query to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the query</returns>
    Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : IQuery<TResult>;
        
    /// <summary>
    /// Dispatches a query and returns the result, inferring the result type from the query
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the query</typeparam>
    /// <param name="query">The query to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the query</returns>
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
