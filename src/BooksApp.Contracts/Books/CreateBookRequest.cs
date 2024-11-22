using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Books;

public class CreateBookRequest
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<Guid> GenreIds { get; init; }
    public required IFormFile Cover { get; init; }
}