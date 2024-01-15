namespace PostsApp.Contracts.Responses.User;

public class UsersResponse : DefaultPagination
{
    public DefaultUserResponse[] users { get; set; }
}