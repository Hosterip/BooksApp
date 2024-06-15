using MediatR;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Images.Commands.CreateImage;

internal sealed class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, string>
{
    public async Task<string> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fileName = $"{DateTime.Now:dd-MM-yy-mm-ss}_{request.Image.FileName.ToLower().Replace(" ", "-")}";
        var filePath = Path.Combine(path, $"{fileName}");
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await request.Image.CopyToAsync(fileStream, cancellationToken);
        return fileName;
    }
}