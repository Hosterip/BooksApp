using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Posts.Results;

public class PostResult : PostWithoutUser
{
    public UserResult User { get; set; }
}