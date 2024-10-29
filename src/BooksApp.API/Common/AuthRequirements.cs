using BooksApp.API.Common.Requirements;

namespace BooksApp.API.Common;

public static class AuthRequirements
{
    public static readonly NotAuthorizedRequirement NotAuthorized = new();
}