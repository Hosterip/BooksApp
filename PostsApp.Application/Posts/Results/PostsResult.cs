using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Posts.Results;

public class PostsResult : DefaultPagination
{
    public PostResult[] posts { get; set; }
}