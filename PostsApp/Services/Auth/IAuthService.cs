using PostsApp.Requests.Auth;

namespace PostsApp.Services.Auth;

public interface IAuthService
{
    public Task Register(AuthPostRequest request);
    public void Login(AuthPostRequest request);
}