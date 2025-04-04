using MediatR;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<AuthResult>
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}