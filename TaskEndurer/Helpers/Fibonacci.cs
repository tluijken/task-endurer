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
    public static int CalculateNumberAtIndex(int index)
    {
        return index switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(index)),
            0 => 0,
            1 => 1,
            _ => CalculateNumberAtIndex(index - 1) + CalculateNumberAtIndex(index - 2)
        };
    }
}