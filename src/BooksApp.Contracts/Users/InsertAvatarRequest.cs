using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Users;

public class InsertAvatarRequest
{
    public required IFormFile Image { get; init; }
}