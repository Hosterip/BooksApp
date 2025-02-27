namespace BooksApp.Application.Common.Constants.ValidationMessages;

public partial class ValidationMessages
{
    public static class Auth
    {
        public const string Occupied = "Email is occupied";
        public const string Password = "Password is wrong";
        public const string EmailOrPassword = "Email or password is wrong";
    }
}