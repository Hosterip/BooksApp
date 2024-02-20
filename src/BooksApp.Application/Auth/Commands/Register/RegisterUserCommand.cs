using MediatR;

namespace PostsApp.Application.Auth.Commands.Register;

public sealed class RegisterUserCommand : IRequest<AuthResult>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
} 