using Microsoft.AspNetCore.Http;

namespace BooksApp.Contracts.Requests.Users;

public class InsertAvatarRequest
{
    public IFormFile Image { get; set; }
}