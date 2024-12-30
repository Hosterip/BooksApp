using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Images.Results;
using MediatR;

namespace BooksApp.Application.Images.Queries.GetImage;

internal sealed class GetImageQueryHandler(IImageFileBuilder imageFileBuilder)
    : IRequestHandler<GetImageQuery, ImageResult>
{
    public async Task<ImageResult> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        var (fileInfo, fileStream) = imageFileBuilder.RetrieveImage(request.ImageName);
        return new ImageResult
        {
            FileInfo = fileInfo,
            FileStream = fileStream
        };
    }
}