namespace TaskEndurer;

/// <summary>
///     A retry strategy that uses a polynomial backoff with a given exponential factor.
/// </summary>
/// <param name="PolynomialFactor">
///     The exponential factor to use for the polynomial backoff.
/// </param>
public record PolynomialBackoffStrategy(double PolynomialFactor);