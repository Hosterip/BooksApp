using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Requests.Books;

public class UpdateBookRequest
{
    public required Guid Id { get; init; }
    public required string? Title { get; init; }
    public required string? Description { get; init; }
    public required IFormFile? Cover { get; init; }
    public required List<Guid> GenreIds { get; init; }
}