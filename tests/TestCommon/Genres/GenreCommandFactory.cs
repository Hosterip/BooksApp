using BooksApp.Application.Genres.Commands.CreateGenre;
using TestCommon.Common.Constants;

namespace TestCommon.Genres;

public static class GenreCommandFactory
{
    public static CreateGenreCommand CreateCreateGenreCommand(
        string name = Constants.Genres.Name)
    {
        return new CreateGenreCommand
        {
            Name = name
        };
    }
}