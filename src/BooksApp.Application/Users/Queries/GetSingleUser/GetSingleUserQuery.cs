using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQuery : IRequest<UserResult>
{
    public required int Id { get; init; }
}