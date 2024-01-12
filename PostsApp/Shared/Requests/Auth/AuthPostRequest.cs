using System.ComponentModel.DataAnnotations;

namespace PostsApp.Requests.Auth;

public class AuthPostRequest
{
    [Required]
    public string username { get; set; }
    [Required]
    public string password { get; set; }
}