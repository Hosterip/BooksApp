using BooksApp.Application.Bookshelves.Commands.RemoveBook;
using BooksApp.Application.Bookshelves.Commands.RemoveBookByName;
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
    
    public static RemoveBookByNameCommand CreateRemoveBookByNameCommand(
        Guid? bookId = null,
        string bookshelfName = Constants.Bookshelves.Name)
    {
        return new RemoveBookByNameCommand
        {
            BookId = bookId ?? Guid.NewGuid(),
            BookshelfName = bookshelfName
        };
    }
    
    public static RemoveBookCommand CreateRemoveBookCommand(
        Guid? bookId = null,
        Guid? bookshelfId = null)
    {
        return new RemoveBookCommand
        {
            BookId = bookId ?? Guid.NewGuid(),
            BookshelfId = bookshelfId ?? Guid.NewGuid()
        };
    }
}