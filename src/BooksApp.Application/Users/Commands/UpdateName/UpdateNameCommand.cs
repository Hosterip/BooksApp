using MediatR;

namespace BooksApp.Application.Users.Commands.UpdateName;

public class UpdateNameCommand : IRequest
{
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
}