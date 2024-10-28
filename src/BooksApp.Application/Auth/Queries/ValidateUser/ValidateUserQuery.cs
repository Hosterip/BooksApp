using MediatR;

namespace PostsApp.Application.Auth.Queries.ValidateUser;

// Returns Role
public class ValidateUserQuery : IRequest<string?>
{
    public required Guid UserId { get; init; }
    public required string SecurityStamp { get; init; }
}