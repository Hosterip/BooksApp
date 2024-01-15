using PostsApp.Contracts.Responses.Post;

namespace PostsApp.Contracts.Responses.User;

public class UserByUsernameRes : DefaultUserResponse
{ 
    public PostResponse[] posts { get; set; }
}