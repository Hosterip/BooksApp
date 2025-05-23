using Bogus;
using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Books.Commands.DeleteBook;
using BooksApp.Application.Books.Commands.PrivilegedDeleteBook;
using BooksApp.Application.Books.Commands.UpdateBook;
using Microsoft.AspNetCore.Http;
using TestCommon.Genres;
using TestCommon.Images;

namespace TestCommon.Books;

public static class BookCommandFactory
{
    public static CreateBookCommand CreateCreateBookCommand(
        string? title = null,
        string? description = null,
        List<Guid>? genres = null,
        IFormFile? image = null)
    {
        return new Faker<CreateBookCommand>()
            .RuleFor(x => x.Title, f => title ?? f.Lorem.Sentence())
            .RuleFor(x => x.Description, f => description ?? f.Lorem.Paragraph())
            .RuleFor(x => x.GenreIds, _ => genres ?? [GenreFactory.CreateGenre().Id.Value])
            .RuleFor(x => x.Image, _ => image ?? ImageFactory.CreateFormFileImage());
    }

    public static UpdateBookCommand CreateUpdateBookCommand(
        Guid? bookId = null,
        string? title = null,
        string? description = null,
        List<Guid>? genres = null,
        IFormFile? image = null)
    {
        return new Faker<UpdateBookCommand>()
            .RuleFor(x => x.Id, f => bookId ?? f.Random.Guid())
            .RuleFor(x => x.Title, f => title ?? f.Lorem.Sentence())
            .RuleFor(x => x.Description, f => description ?? f.Lorem.Paragraph())
            .RuleFor(x => x.GenreIds, _ => genres ?? [GenreFactory.CreateGenre().Id.Value])
            .RuleFor(x => x.Image, _ => image ?? ImageFactory.CreateFormFileImage());
    }

    public static DeleteBookCommand CreateDeleteBookCommand(
        Guid? bookId = null
    )
    {
        return new Faker<DeleteBookCommand>()
            .RuleFor(x => x.Id, f => bookId ?? f.Random.Guid());
    }

    public static PrivilegedDeleteBookCommand CreatePrivilegedDeleteBookCommand(
        Guid? bookId = null
    )
    {
        return new Faker<PrivilegedDeleteBookCommand>()
            .RuleFor(x => x.Id, f => bookId ?? f.Random.Guid());
    }
}