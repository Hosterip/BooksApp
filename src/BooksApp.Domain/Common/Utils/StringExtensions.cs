namespace PostsApp.Domain.Common.Utils;

public static class StringExtensions
{
    public static string ConvertToReferencial(this string str)
    {
        return string.Join("-", str.Split(" ").Select(word => word.Trim()));
    }
}