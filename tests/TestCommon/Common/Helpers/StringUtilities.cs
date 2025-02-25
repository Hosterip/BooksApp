using System.Text;

namespace TestCommon.Common.Helpers;

public static class StringUtilities 
{
    public static string ExceedMaxStringLength(int length)
    {
        var builder = new StringBuilder();
        builder.Append('1', length == 0 
            ? length 
            : length + 1);
        return builder.ToString();
    }
}