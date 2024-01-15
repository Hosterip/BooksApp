using PostsApp.Application.Results.User;
using PostsApp.Contracts.Responses.User;

namespace PostsApp.Contracts.Responses.Post;

public class PostResult
{
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public DefaultUserResult user { get; set; }
}