namespace BooksApp.Contracts.Users;

public sealed class ViewerRelationshipResponse
{
    public required bool IsFollowing { get; init; }
    public required bool IsFriend { get; init; }
    public required bool IsMe { get; init; }
}