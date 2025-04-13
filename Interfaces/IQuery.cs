namespace Devsquare.EasyCQRS.Interfaces;

/// <summary>
/// Interface for queries that return a result
/// </summary>
/// <typeparam name="TResult">The type of result returned by the query</typeparam>
public interface IQuery<out TResult>
{
}
