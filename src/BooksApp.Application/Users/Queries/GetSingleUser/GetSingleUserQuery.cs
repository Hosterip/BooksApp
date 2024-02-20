using MediatR;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQuery : IRequest<SingleUserResult>
{
    public required int Id { get; init; }
}