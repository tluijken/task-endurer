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
    public static int CalculateNumberAtIndex(int index) =>
        index switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(index), "The index must be greater than or equal to zero."),
            0 => 0,
            _ => Enumerable.Range(0, index + 1).Aggregate((previous: 0, current: 1), (previousNumbers, current) =>
                    current < 2 ? previousNumbers : (previousNumbers.current, previousNumbers.previous + previousNumbers.current))
                .current
        };
}