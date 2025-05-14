using System.Net.Mail;

namespace BooksApp.Domain.Common.Helpers;

public static class EmailValidator
{
    public static bool Validate(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith(".")) return false; // suggested by @TK-421
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}