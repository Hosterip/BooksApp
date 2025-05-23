namespace BooksApp.API.Common.Constants;

public static class ApiRoutes
{
    private const string ApiBase = "api";
    private const string ApiPrivilegedBase = "api/privileged";

    public static class Auth
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
        public const string Logout = $"{Base}/logout";
        public const string UpdatePassword = $"{Base}/password";
    }

    public static class Books
    {
        private const string Base = $"{ApiBase}/books";
        private const string PrivilegedBase = $"{ApiPrivilegedBase}/books";

        public const string GetMany = $"{Base}";
        public const string GetSingle = $"{Base}/{{bookId:guid}}";

        public const string Create = $"{Base}";

        public const string Update = $"{Base}";

        public const string Delete = $"{Base}/{{bookId:guid}}";
        public const string PrivilegedDelete = $"{PrivilegedBase}/{{bookId:guid}}";

        // Bookshelves

        public const string AddBook = $"{Base}/{{bookId:guid}}/bookshelves/{{idOrName}}";

        public const string RemoveBook = $"{Base}/{{bookId:guid}}/bookshelves/{{idOrName}}";

        // Reviews 

        public const string GetReviews = $"{Base}/{{bookId:guid}}/reviews";
    }

    public static class Bookshelves
    {
        private const string Base = $"{ApiBase}/bookshelves";

        public const string GetBooks = $"{Base}/{{bookshelfId:guid}}/books/";

        public const string Create = $"{Base}";

        public const string Update = $"{Base}/{{bookshelfId:guid}}/{{newName}}";

        public const string Remove = $"{Base}/{{bookshelfId:guid}}";
    }

    public static class Genres
    {
        private const string Base = $"{ApiBase}/genres";

        public const string Create = $"{Base}/{{name}}";
        public const string GetAll = $"{Base}";
    }

    public static class Images
    {
        private const string Base = $"{ApiBase}/images";

        public const string Get = $"{Base}/{{name}}";
    }

    public static class Reviews
    {
        private const string Base = $"{ApiBase}/reviews";
        private const string PrivilegedBase = $"{ApiPrivilegedBase}";

        public const string Create = $"{Base}";

        public const string Update = $"{Base}";

        public const string Delete = $"{Base}/{{id:guid}}";
        public const string PrivilegedDelete = $"{PrivilegedBase}/{{id:guid}}";
    }

    public static class Users
    {
        private const string Base = $"{ApiBase}/users";

        public const string GetMe = $"{Base}/me";
        public const string GetMany = $"{Base}";
        public const string GetById = $"{Base}/{{userId:guid}}";
        public const string Delete = $"{Base}";
        public const string UpdateEmail = $"{Base}/email";
        public const string UpdateName = $"{Base}/name";
        public const string UpdateAvatar = $"{Base}/avatar";

        // Roles

        public const string GetRoles = $"{Base}/roles";
        public const string UpdateRole = $"{Base}/roles";

        // Bookshelves 

        public const string GetBookshelves = $"{Base}/{{userId:guid}}/bookshelves";
        public const string GetBookshelf = $"{Base}/{{userId:guid}}/bookshelves/{{idOrName}}";

        // Books

        public const string GetManyBooks = $"{Base}/{{userId:guid}}/books";

        // Followers

        public const string AddRemoveFollower = $"{Base}/{{followingId:guid}}/followers";
        public const string GetFollowers = $"{Base}/{{userId:guid}}/followers";
        public const string GetFollowing = $"{Base}/{{userId:guid}}/following";
    }

    public static class Error
    {
        private const string Base = $"/{ApiBase}/error";

        public const string ErrorHandler = $"{Base}";
    }
}