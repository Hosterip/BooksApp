using MediatR;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<AuthResult>
{
    public required Guid Id { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}