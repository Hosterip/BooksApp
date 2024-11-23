namespace BooksApp.Contracts.Users;

public class ExtendedUserResponse : UserResponse
{
    public required ViewerRelationshipResponse? ViewerRelationship { get; init; }
}