namespace BooksApp.Application.Common.Results;

public class ViewerRelationship
{
    public required bool IsFollowing { get; init; }
    public required bool IsFriend { get; init; }
    public required bool IsMe { get; init; }
}