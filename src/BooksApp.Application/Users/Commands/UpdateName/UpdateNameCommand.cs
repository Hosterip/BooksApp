using MediatR;

namespace PostsApp.Application.Users.Commands.UpdateName;

public class UpdateNameCommand : IRequest
{
    public required Guid UserId { get; init; }
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
}