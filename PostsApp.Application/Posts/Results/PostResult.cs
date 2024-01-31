using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Posts.Results;

public record PostResult : PostWithoutUser
{
    public UserResult User { get; set; }
}