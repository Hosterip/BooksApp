using MediatR;

namespace BooksApp.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public required Guid UserId { get; init; }
}