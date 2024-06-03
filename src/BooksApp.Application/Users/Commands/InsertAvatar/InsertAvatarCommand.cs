using MediatR;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommand : IRequest<UserResult>
{
    public required int Id { get; init; }
    public required string ImageName { get; init; }
}