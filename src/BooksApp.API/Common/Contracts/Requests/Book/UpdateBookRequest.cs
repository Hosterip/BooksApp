namespace PostsApp.Common.Contracts.Requests.Book;

public class UpdateBookRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Cover { get; set; }
}