namespace PostsApp.Domain.Constants.Exceptions;

public static class UserExceptionConstants
{
    public static string NotFound = "User not found";
    public static string Occupied = "Username is occupied";
    public static readonly string NotAdmin = "User must be an admin";
    public static readonly string Permission = "Permission denied";
}