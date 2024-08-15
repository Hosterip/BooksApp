namespace PostsApp.Common.Contracts.Requests.Bookshelf;

public class AddBookToDefaultBookshelfRequest
{
    public Guid BookId { get; set; }
    public string BookshelfName { get; set; }
}