namespace PostsApp.Application.Interfaces;

public interface IAuthService
{
    public Task Register(string username, string password);
    public void Login(string username, string password);
}