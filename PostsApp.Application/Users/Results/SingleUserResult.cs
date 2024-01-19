using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Results;

public class SingleUserResult : UserResult
{ 
    public PostWithoutUser[] Posts { get; set; }
}