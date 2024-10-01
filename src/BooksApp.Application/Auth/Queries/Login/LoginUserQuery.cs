using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Auth.Queries.Login;

public class LoginUserQuery : IRequest<AuthResult>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}