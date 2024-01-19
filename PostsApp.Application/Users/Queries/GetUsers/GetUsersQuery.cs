using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<PaginatedArray<UserResult>>
{
    public string? Query { get; set; }
    public int Page { get; set; }
    public int? Limit { get; set; }
}