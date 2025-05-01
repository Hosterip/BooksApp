using BooksApp.Application.Common.Attributes;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Roles.Commands.UpdateRole;

[Authorize]
public sealed class UpdateRoleCommand : IRequest<UserResult>
{
    public required Guid UserId { get; init; }
    public required string Role { get; init; }
}