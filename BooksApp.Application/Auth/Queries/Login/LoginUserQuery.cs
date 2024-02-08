using MediatR;

namespace PostsApp.Application.Auth.Queries.Login;

public class LoginUserQuery : IRequest<AuthResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}