using MediatR;
using PostsApp.Application.Images.Results;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Images.Queries.GetImage;

internal sealed class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageResult>
{
    public async Task<ImageResult> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        var uploadPath = 
            Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images");
        var filePath = Path.Combine(uploadPath, request.ImageName);
        var fileInfo = new FileInfo(filePath);
        var fileStream = new FileStream(filePath, FileMode.Open);
        return new ImageResult
        {
            FileInfo = fileInfo,
            FileStream = fileStream
        };
    }
}