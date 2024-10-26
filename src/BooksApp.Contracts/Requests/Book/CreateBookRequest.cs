using Microsoft.AspNetCore.Http;

namespace PostsApp.Common.Contracts.Requests.Book;

public class CreateBookRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Guid> GenreIds { get; set; }
    public IFormFile Cover { get; set; }
}