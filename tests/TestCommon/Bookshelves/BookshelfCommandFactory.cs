using BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;
using TestCommon.Common.Constants;

namespace TestCommon.Bookshelves;

public static class BookshelfCommandFactory
{
    public static UpdateBookshelfNameCommand CreateUpdateBookshelfNameCommand(
        string newName = Constants.Bookshelves.Name + "Foo",
        Guid? bookshelfId = null)
    {
        return new UpdateBookshelfNameCommand
        {
            NewName = newName,
            BookshelfId = bookshelfId ?? Guid.NewGuid()
        };
    }
}