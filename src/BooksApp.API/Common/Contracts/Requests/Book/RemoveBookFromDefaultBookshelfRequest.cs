namespace PostsApp.Common.Contracts.Requests.Bookshelf;

public class RemoveBookFromDefaultBookshelfRequest
{
    public Guid BookId { get; set; }
    public string BookshelfName { get; set; }
}