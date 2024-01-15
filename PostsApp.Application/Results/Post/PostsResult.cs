namespace PostsApp.Contracts.Responses.Post;

public class PostsResult : DefaultPagination
{
    public PostResult[] posts { get; set; }
}