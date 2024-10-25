using MediatR;
using Microsoft.AspNetCore.Http;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommand : IRequest<UserResult>
{
    public required Guid Id { get; init; }
    public required IFormFile Image { get; init; }
}