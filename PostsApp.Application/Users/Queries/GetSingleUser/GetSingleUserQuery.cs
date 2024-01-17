using MediatR;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQuery : IRequest<SingleUserResult>
{
    public string Username { get; set; }
}