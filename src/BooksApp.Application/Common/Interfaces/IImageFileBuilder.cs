using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Common.Interfaces;

public interface IImageFileBuilder
{
    Task<string> CreateImage(IFormFile file, CancellationToken token = default);
    bool DeleteImage(string fileName);
    (FileInfo fileInfo, FileStream fileStream) RetrieveImage(string fileName);
    bool AnyImage(string fileName);
    bool IsValid(string fileName);
}