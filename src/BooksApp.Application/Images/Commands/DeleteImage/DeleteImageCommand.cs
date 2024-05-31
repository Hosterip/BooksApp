using MediatR;

namespace PostsApp.Application.Images.Commands.DeleteImage;

public class DeleteImageCommand : IRequest
{
    public required string ImageName { get; set; }
}