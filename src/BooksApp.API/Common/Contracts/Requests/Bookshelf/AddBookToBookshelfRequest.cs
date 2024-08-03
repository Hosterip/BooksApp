namespace PostsApp.Common.Contracts.Requests.Bookshelf;

public class AddBookToBookshelfRequest
{
    public Guid BookshelfId { get; set; }
    public Guid BookId { get; set; }
}