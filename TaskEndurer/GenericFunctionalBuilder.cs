namespace TaskEndurer;

/// <summary>
///   A generic builder that can be used to build a subject in a very fluent way.
/// </summary>
/// <typeparam name="TSubject">
///     The subject to build.
/// </typeparam>
/// <typeparam name="TSelf">
///     The type of the builder.
/// </typeparam>
public abstract class GenericFunctionalBuilder<TSubject, TSelf>
    where TSubject : new()
    where TSelf : GenericFunctionalBuilder<TSubject, TSelf>
{
    /// <summary>
    ///     To hold a list fluent actions.
    /// </summary>
    private readonly List<Func<TSubject, TSubject>> _actions = new();

    /// <summary>
    ///     Helps in adding functions to a list of action.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    protected TSelf Do(Action<TSubject> action) => AddActions(action);

    /// <summary>
    ///     Adds all functions to a list
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private TSelf AddActions(Action<TSubject> action)
    {
        _actions.Add(p =>
        {
            action(p);
            return p;
        });
        return (TSelf)this;
    }

    /// <summary>
    ///     Builds Subject In Very Fluent Way
    /// </summary>
    /// <returns></returns>
    protected TSubject Build() => _actions.Aggregate(new TSubject(), (p, f) => f(p));
}