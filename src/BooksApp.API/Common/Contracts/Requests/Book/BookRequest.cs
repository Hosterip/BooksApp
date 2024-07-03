using System.Runtime.InteropServices.JavaScript;

namespace PostsApp.Common.Contracts.Requests.Book;

public class BookRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Guid> GenreIds { get; set; }
    public IFormFile Cover { get; set; }
}