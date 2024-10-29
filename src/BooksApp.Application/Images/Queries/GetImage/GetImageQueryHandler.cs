using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Images.Results;
using MediatR;

namespace BooksApp.Application.Images.Queries.GetImage;

internal sealed class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageResult>
{
    private readonly IImageFileBuilder _imageFileBuilder;

    public GetImageQueryHandler(IImageFileBuilder imageFileBuilder)
    {
        _imageFileBuilder = imageFileBuilder;
    }

    public async Task<ImageResult> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        var (fileInfo, fileStream) = await _imageFileBuilder.RetrieveImage(request.ImageName, cancellationToken);
        return new ImageResult
        {
            FileInfo = fileInfo,
            FileStream = fileStream
        };
    }
}