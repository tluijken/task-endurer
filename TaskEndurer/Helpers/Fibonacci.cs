namespace TaskEndurer.Helpers;

internal static class Fibonacci
{
    /// <summary>
    ///     Calculates the Fibonacci number at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the Fibonacci number to calculate.
    /// </param>
    /// <returns>
    ///     The Fibonacci number at the specified index.
    /// </returns>
    public static long CalculateNumberAtIndex(uint index) =>
        index switch
        {
            0 => 0,
            _ => Enumerable.Range(0, (int)(index + 1)).Aggregate((previous: 0, current: 1), (prev, curr) =>
                    curr < 2 ? prev : (prev.current, prev.previous + prev.current))
                .current
        };
}