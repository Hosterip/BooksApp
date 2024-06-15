using MediatR;
using PostsApp.Domain.User;

namespace PostsApp.Application.Auth.Queries.GetFullUser;

public class GetFullUserQuery : IRequest<User?>
{
    public required Guid UserId { get; init; }
}