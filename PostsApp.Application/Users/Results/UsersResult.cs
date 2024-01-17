using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Results;

public class UsersResult : DefaultPagination
{
    public DefaultUserResult[] users { get; set; }
}