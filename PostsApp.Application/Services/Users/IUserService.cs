using PostsApp.Contracts.Responses.User;
using PostsApp.Models;

namespace PostsApp.Application.Services.Users;

public interface IUserService
{
    public Task AddUser(string username, string hash, string salt);
    public Task DeleteUser(string username);
    public UsersResult GetUsers(int limit, int page, string query);
    public User GetSingleUserOrBadRequest(string username, string error);
    public UserByUsernameRes GetUserByUsername(string username);
}