using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Requests.Books;

public class CreateBookRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Guid> GenreIds { get; set; }
    public IFormFile Cover { get; set; }
}