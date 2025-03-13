using System.Text.RegularExpressions;

namespace BooksApp.Domain.Common.Helpers;

public static partial class GenerateRefNameExtension
{
    public static string GenerateRefName(this string name)
    {
        var refName = RefNameRegex()
            .Replace(name, string.Empty)
            .Replace(" ", "-")
            .ToLower();
        return $"{refName}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex RefNameRegex();
}