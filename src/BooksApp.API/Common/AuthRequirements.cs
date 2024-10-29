using BooksApp.API.Common.Requirements;

namespace BooksApp.API.Common;

public static class AuthRequirements
{
    public static readonly AuthorizedRequirement Authorized = new();
    public static readonly NotAuthorizedRequirement NotAuthorized = new();
}