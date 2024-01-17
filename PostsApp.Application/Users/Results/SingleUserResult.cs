using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Users.Results;

public class SingleUserResult : DefaultUserResult
{ 
    public PostWithoutUser[] posts { get; set; }
}