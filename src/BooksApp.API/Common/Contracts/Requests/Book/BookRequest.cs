namespace PostsApp.Common.Contracts.Requests.Book;

public class BookRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Cover { get; set; }
}