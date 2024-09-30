using MediatR;

namespace PostsApp.Application.Auth.Commands.Register;

public sealed class RegisterUserCommand : IRequest<AuthResult>
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public required string Password { get; init; }
} 