using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Users.Commands.DeleteUser;

[Authorize]
public class DeleteUserCommand : IRequest
{
    public required Guid UserId { get; init; }
}