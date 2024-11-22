using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Books;

public class UpdateBookRequest
{
    public required Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public IFormFile? Cover { get; init; }
    public required List<Guid> GenreIds { get; init; }
}