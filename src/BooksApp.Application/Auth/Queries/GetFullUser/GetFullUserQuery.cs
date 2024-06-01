using MediatR;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Auth.Queries.IsSessionValid;

public class GetFullUserQuery : IRequest<User?>
{
    public required int UserId { get; init; }
}