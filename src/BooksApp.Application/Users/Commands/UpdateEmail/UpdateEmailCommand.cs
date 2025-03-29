using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

public class UpdateEmailCommand : IRequest
{
    public required string Email { get; init; }
}