using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Results;

public class SingleUserResult : UserResult
{ 
    public required BookResult[] Posts { get; init; }
}