using BooksApp.Application.Genres;
using TestCommon.Common.Constants;

namespace TestCommon.Genres;

public static class GenreResultFactory
{
    public static GenreResult CreateGenreResult(
        Guid? id = null,
        string name = Constants.Genres.Name)
    {
        return new GenreResult
        {
            Id = id ?? Guid.NewGuid(),
            Name = name
        };
    }
}