using System.Text;

namespace TestCommon.Common.Helpers;

public static class StringUtilities 
{
    public static string GenerateLongString(int length)
    {
        var builder = new StringBuilder();
        builder.Append('1', length);
        return builder.ToString();
    }
    
    public static string GenerateLongWhiteSpace(int length)
    {
        var builder = new StringBuilder();
        builder.Append(' ', length);
        return builder.ToString();
    }
}