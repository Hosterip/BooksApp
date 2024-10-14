namespace PostsApp.Domain.Common.Constants;

public static class DefaultBookshelvesNames
{
    public const string CurrentlyReading = "currently-reading";
    public const string ToRead = "to-read";
    public const string Read = "read";

    public static readonly HashSet<string> AllValues =
    [
        Read,
        ToRead,
        CurrentlyReading
    ];
}