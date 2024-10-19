namespace PostsApp.Common.Constants;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
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

        public const string GetMany = $"{Base}";
        public const string GetSingle = $"{Base}/{{id:guid}}";
        
        public const string Create = $"{Base}";
        
        public const string Update = $"{Base}";
        
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Bookshelves
    {
        private const string Base = $"{ApiBase}/bookshelves";

        public const string GetBookshelves = $"{Base}/{{userId:guid}}";
        public const string GetBooks = $"{Base}/books/{{bookshelfId:guid}}";
        
        public const string Create = $"{Base}/{{name}}";
        public const string AddBook = $"{Base}/book";
        public const string AddBookToDefault = $"{Base}/bookToDefault";
        
        public const string RemoveBook = $"{Base}/book";
        public const string RemoveBookFromDefault = $"{Base}/bookFromDefault";
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

        public const string GetMany = $"{Base}/many/{{id:guid}}";
        
        public const string Create = $"{Base}";
        
        public const string Update = $"{Base}";
        
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Roles
    {
        private const string Base = $"{ApiBase}/roles";

        public const string GetAll = $"{Base}";
        public const string UpdateRole = $"{Base}";
    }
    
    public static class Users
    {
        private const string Base = $"{ApiBase}/users";
        
        public const string GetMe = $"{Base}/me";
        public const string GetMany = $"{Base}";
        public const string GetById = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}";
        public const string UpdateEmail = $"{Base}/email";
        public const string UpdateName = $"{Base}/name";
        public const string UpdateAvatar = $"{Base}/avatar";
    }

    public static class Error
    {
        private const string Base = $"{ApiBase}/error";

        public const string ErrorHandler = $"{Base}";
    }
}