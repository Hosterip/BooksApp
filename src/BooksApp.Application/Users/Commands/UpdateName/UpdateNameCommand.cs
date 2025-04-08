using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateName;

[Authorize]
public class UpdateNameCommand : IRequest
{
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
}