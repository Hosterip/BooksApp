namespace BooksApp.Application.Images.Results;

public class ImageResult
{
    public required FileInfo FileInfo { get; init; }
    public required FileStream FileStream { get; init; }
}