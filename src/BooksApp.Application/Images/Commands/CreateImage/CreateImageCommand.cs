using MediatR;
using Microsoft.AspNetCore.Http;

namespace PostsApp.Application.Images.Commands.CreateImage;

public class CreateImageCommand : IRequest<string>
{
    public required IFormFile Image { get; init; }
}