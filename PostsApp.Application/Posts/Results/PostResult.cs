using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Posts.Results;

public class PostResult : PostWithoutUser
{
    public DefaultUserResult user { get; set; }
}