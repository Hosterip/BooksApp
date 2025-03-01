using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Books.Commands.DeleteBook;
using BooksApp.Application.Books.Commands.PrivilegedDeleteBook;
using BooksApp.Application.Books.Commands.UpdateBook;
using Microsoft.AspNetCore.Http;
using TestCommon.Common.Constants;
using TestCommon.Genres;
using TestCommon.Images;

namespace TestCommon.Books;

public static class BookCommandFactory
{
    public static CreateBookCommand CreateCreateBookCommand(
        string title = Constants.Books.Title, 
        string description = Constants.Books.Description,
        List<Guid>? genres = null,
        IFormFile? image = null)
    {
        return new CreateBookCommand
        {
            Title = title,
            Description = description,
            GenreIds = genres ?? [GenreFactory.CreateGenre().Id.Value],
            Image = image ?? ImageFactory.CreateFormFileImage()
        };
    }
    
    public static UpdateBookCommand CreateUpdateBookCommand(
        Guid? bookId = null,
        string title = Constants.Books.Title, 
        string description = Constants.Books.Description,
        List<Guid>? genres = null,
        IFormFile? image = null)
    {
        return new UpdateBookCommand
        {
            Id = bookId ?? Guid.NewGuid(),
            Title = title,
            Description = description,
            GenreIds = genres ?? [GenreFactory.CreateGenre().Id.Value],
            Image = image ?? ImageFactory.CreateFormFileImage()
        };
    }
    
    public static DeleteBookCommand CreateDeleteBookCommand(
        Guid? bookId = null
    )
    {
        return new DeleteBookCommand
        {
            Id = bookId ?? Guid.NewGuid()
        };
    }
    
    public static PrivilegedDeleteBookCommand CreatePrivilegedDeleteBookCommand(
        Guid? bookId = null
    )
    {
        return new PrivilegedDeleteBookCommand
        {
            Id = bookId ?? Guid.NewGuid()
        };
    }
}