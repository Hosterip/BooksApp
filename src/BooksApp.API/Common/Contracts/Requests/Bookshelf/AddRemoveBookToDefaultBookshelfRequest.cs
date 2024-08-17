namespace PostsApp.Common.Contracts.Requests.Bookshelf;

public class AddRemoveBookToDefaultBookshelfRequest
{
    public Guid BookId { get; set; }
    public string BookshelfName { get; set; }
}