using PostsApp.Application.Results.User;

namespace PostsApp.Contracts.Responses.User;

public class UsersResult : DefaultPagination
{
    public DefaultUserResult[] users { get; set; }
}