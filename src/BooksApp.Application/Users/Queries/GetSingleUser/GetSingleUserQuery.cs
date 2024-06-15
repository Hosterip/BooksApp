using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQuery : IRequest<UserResult>
{
    public required Guid Id { get; init; }
}