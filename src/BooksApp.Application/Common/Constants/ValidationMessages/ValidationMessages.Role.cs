namespace BooksApp.Application.Common.Constants.ValidationMessages;

public partial class ValidationMessages
{
    public static class Role
    {
        public const string NotFound = "Role not found";
        public const string CanNotChangeYourOwn = "You can't change your own role";
    }
}