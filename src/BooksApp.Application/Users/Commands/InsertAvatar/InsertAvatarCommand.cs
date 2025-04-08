using BooksApp.Application.Common.Attributes;
using BooksApp.Application.Users.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Users.Commands.InsertAvatar;

[Authorize]
public class InsertAvatarCommand : IRequest<UserResult>
{
    public required IFormFile Image { get; init; }
}