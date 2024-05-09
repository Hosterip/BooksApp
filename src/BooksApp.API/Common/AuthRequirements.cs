using PostsApp.Common.Requirements;

namespace PostsApp.Common;

public static class AuthRequirements
{
    public static readonly  AuthorizedRequirement Authorized = new AuthorizedRequirement();
    public static readonly  NotAuthorizedRequirement NotAuthorized = new NotAuthorizedRequirement();
}