using BooksApp.Application.Images.Results;
using MediatR;

namespace BooksApp.Application.Images.Queries.GetImage;

public sealed class GetImageQuery : IRequest<ImageResult>
{
    public required string ImageName { get; init; }
}