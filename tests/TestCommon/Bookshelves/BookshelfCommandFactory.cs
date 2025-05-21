using Bogus;
using BooksApp.Application.Bookshelves.Commands.AddBook;
using BooksApp.Application.Bookshelves.Commands.AddBookByName;
using BooksApp.Application.Bookshelves.Commands.CreateBookshelf;
using BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;
using BooksApp.Application.Bookshelves.Commands.RemoveBook;
using BooksApp.Application.Bookshelves.Commands.RemoveBookByName;
using BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;

namespace TestCommon.Bookshelves;

public static class BookshelfCommandFactory
{
    public static UpdateBookshelfNameCommand CreateUpdateBookshelfNameCommand(
        string? newName = null,
        Guid? bookshelfId = null)
    {
        return new Faker<UpdateBookshelfNameCommand>()
            .RuleFor(x => x.BookshelfId, f => bookshelfId ?? f.Random.Guid())
            .RuleFor(x => x.NewName, f => newName ?? f.Lorem.Slug());
    }

    public static RemoveBookByNameCommand CreateRemoveBookByNameCommand(
        Guid? bookId = null,
        string? bookshelfName = null)
    {
        return new Faker<RemoveBookByNameCommand>()
            .RuleFor(x => x.BookId, f => bookId ?? f.Random.Guid())
            .RuleFor(x => x.BookshelfName, f => bookshelfName ?? f.Lorem.Slug());
    }

    public static RemoveBookCommand CreateRemoveBookCommand(
        Guid? bookId = null,
        Guid? bookshelfId = null)
    {
        return new Faker<RemoveBookCommand>()
            .RuleFor(x => x.BookId, f => bookId ?? f.Random.Guid())
            .RuleFor(x => x.BookshelfId, f => bookshelfId ?? f.Random.Guid());
    }

    public static DeleteBookshelfCommand CreateDeleteBookshelfCommand(
        Guid? bookshelfId = null)
    {
        return new Faker<DeleteBookshelfCommand>()
            .RuleFor(x => x.BookshelfId, f => bookshelfId ?? f.Random.Guid());
    }

    public static CreateDefaultBookshelvesCommand CreateCreateDefaultBookshelvesCommand(
        Guid? userId = null)
    {
        return new Faker<CreateDefaultBookshelvesCommand>()
            .RuleFor(x => x.UserId, f => userId ?? f.Random.Guid());
    }

    public static CreateBookshelfCommand CreateCreateBookshelfCommand(
        string? bookshelfName = null)
    {
        return new Faker<CreateBookshelfCommand>()
            .RuleFor(x => x.Name, f => bookshelfName ?? f.Lorem.Slug());
    }

    public static AddBookByNameCommand CreateAddBookByNameCommand(
        Guid? bookId = null,
        string? bookshelfName = null)
    {
        return new Faker<AddBookByNameCommand>()
            .RuleFor(x => x.BookId, f => bookId ?? f.Random.Guid())
            .RuleFor(x => x.BookshelfName, f => bookshelfName ?? f.Lorem.Slug());
    }


    public static AddBookCommand CreateAddBookCommand(
        Guid? bookId = null,
        Guid? bookshelfId = null)
    {
        return new Faker<AddBookCommand>()
            .RuleFor(x => x.BookId, f => bookId ?? f.Random.Guid())
            .RuleFor(x => x.BookshelfId, f => bookshelfId ?? f.Random.Guid());
    }
}