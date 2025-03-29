using BooksApp.Application.Users.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommand : IRequest<UserResult>
{
    public required IFormFile Image { get; init; }
}