using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

[Authorize]
public class ChangePasswordCommand : IRequest<AuthResult>
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}