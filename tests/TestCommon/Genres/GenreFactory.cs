using BooksApp.Domain.Genre;
using TestCommon.Common.Constants;

namespace TestCommon.Genres;

public static class GenreFactory
{
    public static Genre CreateGenre(
        string name = Constants.Genres.Name)
    {
        return Genre.Create(name);
    } 
}