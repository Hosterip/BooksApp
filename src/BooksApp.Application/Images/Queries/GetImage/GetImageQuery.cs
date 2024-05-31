using MediatR;
using PostsApp.Application.Images.Results;

namespace PostsApp.Application.Images.Queries.GetImage;

public sealed class GetImageQuery : IRequest<ImageResult>
{
    public required string ImageName { get; init; }
}