using Microsoft.AspNetCore.Http;

namespace PostsApp.Application.Common.Interfaces;

public interface IImageFileBuilder
{
    Task<string?> CreateImage(IFormFile file, CancellationToken token = default);
    Task<bool> DeleteImage(string fileName, CancellationToken token = default);
    Task<(FileInfo fileInfo, FileStream fileStream)> RetrieveImage(string fileName, CancellationToken token = default);
    bool AnyImage(string fileName, CancellationToken token = default);
    bool IsValid(string fileName, CancellationToken token = default);
}