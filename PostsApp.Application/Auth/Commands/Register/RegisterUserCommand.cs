using MediatR;

namespace PostsApp.Application.Auth.Commands.Register;

public sealed class RegisterUserCommand : IRequest<AuthResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
} 