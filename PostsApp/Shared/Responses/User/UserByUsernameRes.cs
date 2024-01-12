using PostsApp.Shared.Responses.Post;

namespace PostsApp.Shared.Responses.User;

public class UserByUsernameRes : DefaultUserResponse
{ 
    public PostResponse[] posts { get; set; }
}