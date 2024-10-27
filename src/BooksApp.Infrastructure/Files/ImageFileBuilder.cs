using Microsoft.AspNetCore.Http;
using PostsApp.Application.Common.Constants;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Infrastructure.Files;

public class ImageFileBuilder : IImageFileBuilder
{
    public async Task<string?> CreateImage(IFormFile file, CancellationToken token = default)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        var fileName = $"{DateTime.Now:ddMMyymm}_{Guid.NewGuid()}";
        var filePath = Path.Combine(path, $"{fileName}");
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream, token);
        return fileName;
    }

    public async Task<bool> DeleteImage(string fileName, CancellationToken token = default)
    {
        var fileInfo = new FileInfo(
            Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images",
                fileName
            )
        );
        var exists = fileInfo.Exists;
        if (exists) fileInfo.Delete();
        return exists;
    }

    public async Task<(FileInfo fileInfo, FileStream fileStream)> RetrieveImage(string fileName,
        CancellationToken token = default)
    {
        var uploadPath =
            Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images");
        var filePath = Path.Combine(uploadPath, fileName);
        var fileInfo = new FileInfo(filePath);
        var fileStream = new FileStream(filePath, FileMode.Open);

        return (fileInfo, fileStream);
    }

    public bool AnyImage(string fileName, CancellationToken token = default)
    {
        var fileInfo = new FileInfo(
            Path.Combine(Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images",
                fileName
            )
        );
        return fileInfo.Exists;
    }

    public bool IsValid(string fileName, CancellationToken token = default)
    {
        var path = Path.Combine(
            Environment.GetEnvironmentVariable(EnvironmentNames.ImageFolderPath) ?? "images",
            fileName
        );
        var fileInfo = new FileInfo(path);
        var extension = fileInfo.Extension.Replace(".", "");
        return AppConstants.AllowedExtensions.Contains(extension);
    }
}