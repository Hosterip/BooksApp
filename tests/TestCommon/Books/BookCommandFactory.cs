using BooksApp.Application.Books.Commands.CreateBook;
using Microsoft.AspNetCore.Http;
using TestCommon.Common.Constants;
using TestCommon.Genres;
using TestCommon.Images;

namespace TestCommon.Books;

public static class BookCommandFactory
{
    public static CreateBookCommand CreateCreateBookCommand(
        Guid? userId = null,
        string title = Constants.Books.Title, 
        string description = Constants.Books.Description,
        List<Guid>? genres = null,
        IFormFile? image = null)
    {
        return new CreateBookCommand
        {
            UserId = userId ?? Guid.NewGuid(),
            Title = title,
            Description = description,
            GenreIds = genres ?? [GenreFactory.CreateGenre().Id.Value],
            Image = image ?? ImageFactory.CreateFormFileImage()
        };
    }
}