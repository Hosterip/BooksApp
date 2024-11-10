using MediatR;

namespace BooksApp.Application.Auth.Commands.Register;

public sealed class RegisterUserCommand : IRequest<AuthResult>
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string Password { get; init; }
}