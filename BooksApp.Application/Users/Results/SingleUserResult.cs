using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Results;

public class SingleUserResult : UserResult
{ 
    public BookResult[] Posts { get; set; }
    public LikeResult[] Likes { get; set; }
}