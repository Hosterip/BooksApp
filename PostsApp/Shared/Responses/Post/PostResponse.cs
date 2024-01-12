using PostsApp.Shared.Responses.User;

namespace PostsApp.Shared.Responses.Post;

public class PostResponse
{
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public DefaultUserResponse user { get; set; }
}