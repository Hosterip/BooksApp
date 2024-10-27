using MediatR;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQuery : IRequest<UserResult>
{
    public required Guid Id { get; init; }
}