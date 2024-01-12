namespace PostsApp.Shared.Responses.Post;

public class PostsResponse
{
    public int totalPages { get; set; }
    public int totalCount { get; set; }
    public PostResponse[] posts { get; set; }
}