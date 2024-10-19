namespace PostsApp.Common.Contracts.Requests.Book;

public class RemoveBookFromBookshelfRequest
{
    public Guid BookshelfId { get; set; }
    public Guid BookId { get; set; }
}