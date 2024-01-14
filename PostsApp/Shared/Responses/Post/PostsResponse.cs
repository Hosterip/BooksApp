namespace PostsApp.Shared.Responses.Post;

public class PostsResponse : DefaultPagination
{
    public PostResponse[] posts { get; set; }
}