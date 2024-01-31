using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Users.Results;

public class SingleUserResult : UserResult
{ 
    public PostResult[] Posts { get; set; }
    public LikeResult[] Likes { get; set; }
}