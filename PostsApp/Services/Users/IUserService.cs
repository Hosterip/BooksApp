using PostsApp.Models;
using PostsApp.Shared.Responses.User;

namespace PostsApp.Services.Users;

public interface IUserService
{
    public Task AddUser(string username, string hash, string salt);
    public Task DeleteUser(string username);
    public UsersResponse GetUsers(int limit, int page, string query);
    public User GetSingleUserOrBadRequest(string username, string error);
    public UserByUsernameRes GetUserByUsername(string username);
}