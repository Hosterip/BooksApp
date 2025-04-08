using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Users.Commands.AddRemoveFollower;

[Authorize]
public sealed class AddRemoveFollowerCommand : IRequest
{
    public required Guid UserId { get; init; }
}