using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommand : IRequest<UserResult>
{
    public required Guid Id { get; init; }
    public required string ImageName { get; init; }
}