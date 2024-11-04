namespace BooksApp.Application.Common.Constants.ValidationMessages;

public static class UserValidationMessages
{
    public const string NotFound = "User not found";
    public const string Occupied = "Username is occupied";
    public const string Permission = "Permission denied";
    public const string CantFollowYourself = "Can't follow yourself";
    public const string AlreadyFollowing = "You already following this user";
}