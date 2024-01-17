using MediatR;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<UsersResult>
{
    public string? Query { get; set; }
    public int Page { get; set; }
    public int? Limit { get; set; }
}