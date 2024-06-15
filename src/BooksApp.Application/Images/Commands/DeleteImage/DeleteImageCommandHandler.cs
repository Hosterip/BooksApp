using MediatR;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Images.Commands.DeleteImage;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
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