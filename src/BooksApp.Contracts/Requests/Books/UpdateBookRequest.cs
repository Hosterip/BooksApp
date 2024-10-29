using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Requests.Books;

public class UpdateBookRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IFormFile? Cover { get; set; }
    public List<Guid> GenreIds { get; set; }
}