using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

[Authorize]
public class UpdateEmailCommand : IRequest
{
    public required string Email { get; init; }
}