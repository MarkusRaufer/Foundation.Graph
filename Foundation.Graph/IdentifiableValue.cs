namespace Foundation.Graph;

/// <summary>
/// Identifiable decorator for a value. If you have a graph with node ids, you can use this decorator to add the value to the graph.
/// </summary>
/// <typeparam name="TValue">Typeo of the value.</typeparam>
/// <param name="Id"></param>
/// <param name="Value"></param>
public record IdentifiableValue<TValue>(Guid Id, TValue Value)
    : IdentifiableValue<Guid, TValue>(Id, Value);

/// <summary>
/// Identifiable decorator for a value. If you have a graph with node ids, you can use this decorator to add the value to the graph.
/// </summary>
/// <typeparam name="TId">Type of the id.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
/// <param name="Id">This is the identifier.</param>
/// <param name="Value">This is a value.</param>
public record IdentifiableValue<TId, TValue>(TId Id, TValue Value)
    : IIdentifiable<TId>
    where TId : notnull;