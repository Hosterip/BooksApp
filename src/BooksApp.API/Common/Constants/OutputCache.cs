namespace BooksApp.API.Common.Constants;

public static class OutputCache
{
    public static class Books
    {
        public const string PolicyName = "BooksCache";
        public const string Tag = "books";
    }

    public static class Users
    {
        public const string PolicyName = "UsersCache";
        public const string Tag = "users";
    }

    public static class Reviews
    {
        public const string PolicyName = "ReviewsCache";
        public const string Tag = "reviews";
    }

    public static class Bookshelves
    {
        public const string PolicyName = "BookshelvesCache";
        public const string Tag = "bookshelves";
    }

    public static class Genres
    {
        public const string PolicyName = "GenresCache";
        public const string Tag = "genres";
    }
}