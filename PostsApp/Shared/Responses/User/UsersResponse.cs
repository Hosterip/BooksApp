namespace PostsApp.Shared.Responses.User;

public class UsersResponse
{
    public DefaultUserResponse[] users { get; set; }
    public int totalPages { get; set; }
    public int totalCount { get; set; }
}