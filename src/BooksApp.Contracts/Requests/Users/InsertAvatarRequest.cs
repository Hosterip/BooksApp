using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Requests.Users;

public class InsertAvatarRequest
{
    public required IFormFile Image { get; init; }
}