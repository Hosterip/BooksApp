using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Images.Commands.DeleteImage;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
    private readonly IImageFileBuilder _imageFileBuilder;

    public Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var fileInfo = new FileInfo(
                Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images",
                    request.ImageName
                    )
        );
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        return Task.CompletedTask;
    }
}