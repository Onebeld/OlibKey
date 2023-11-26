namespace OlibKey.Core.Helpers;

public static class StringFinder
{
    private static int LevenshteinDistance(string source, string target)
    {
        if (source == target) return 0;
        if (source.Length == 0) return target.Length;
        if (target.Length == 0) return source.Length;

        int[] vector0 = new int[target.Length + 1];
        int[] vector1 = new int[target.Length + 1];

        for (int vector0Index = 0; vector0Index < vector0.Length; vector0Index++)
            vector0[vector0Index] = vector0Index;

        for (int sourceIndex = 0; sourceIndex < source.Length; sourceIndex++)
        {
            vector1[0] = sourceIndex + 1;

            for (int targetIndex = 0; targetIndex < target.Length; targetIndex++)
            {
                int cost = source[sourceIndex] == target[targetIndex] ? 0 : 1;
                vector1[targetIndex + 1] = MathHelpers.GetMinFromNumbers(vector1[targetIndex] + 1, vector0[targetIndex + 1] + 1, vector0[targetIndex] + cost);
            }

            for (int vector0Index = 0; vector0Index < vector0.Length; vector0Index++)
            {
                vector0[vector0Index] = vector1[vector0Index];
            }
        }

        return vector1[target.Length];
    }
    
    /// <summary>
    /// Calculates the percentage similarity of two strings using the Levenshtein distance
    /// </summary>
    /// <param name="source">Source string to compare with</param>
    /// <param name="target">Targeted string to compare</param>
    /// <returns>Similarity between two strings from 0 to 1.0</returns>
    public static double CalculateSimilarity(string? source, string? target)
    {
        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target)) return 0.0;
        if (source == target) return 1.0;

        int stepsToSame = LevenshteinDistance(source, target);
        return 1.0 - (double)stepsToSame / Math.Max(source.Length, target.Length);
    }
}