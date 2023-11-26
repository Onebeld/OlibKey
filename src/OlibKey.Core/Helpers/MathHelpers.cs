namespace OlibKey.Core.Helpers;

public static class MathHelpers
{
    /// <summary>
    /// Gets the minimum number from the array
    /// </summary>
    /// <param name="numbers">Numbers</param>
    /// <returns>Minimum number</returns>
    public static int GetMinFromNumbers(params int[] numbers) => numbers.Min();
}