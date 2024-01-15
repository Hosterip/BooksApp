using PostsApp.Application.Results.User;
using PostsApp.Contracts.Responses.Post;

namespace PostsApp.Contracts.Responses.User;

public class UserByUsernameRes : DefaultUserResult
{ 
    public PostResult[] posts { get; set; }
}