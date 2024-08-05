namespace PostsApp.Common.Contracts.Requests.Bookshelf;

public class AddRemoveBookBookshelfRequest
{
    public Guid BookshelfId { get; set; }
    public Guid BookId { get; set; }
}