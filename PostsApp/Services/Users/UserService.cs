using Microsoft.IdentityModel.Tokens;
using PostsApp.DB;
using PostsApp.Models;
using PostsApp.Shared.Responses.Post;
using PostsApp.Shared.Responses.User;

namespace PostsApp.Services.Users;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUser(string username, string hash, string salt)
    {
        var user = _dbContext.Users.SingleOrDefault(x => x.Username == username); //returns a single item.
        if (user != null) throw new BadHttpRequestException("Username is occupied");
        _dbContext.Users.Add(new User { Username = username, Hash = hash, Salt = salt });
        await _dbContext.SaveChangesAsync();
    }


    public UsersResponse GetUsers(int limit, int page, string query)
    {
        var rawUsers =
            (from user in _dbContext.Users
                where query.IsNullOrEmpty() || user.Username.Contains(query)
                select new DefaultUserResponse { username = user.Username });
        var users = rawUsers
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToArray();
        
        int totalCount = rawUsers.Count();
        int totalPages = (int)Math.Ceiling((decimal)totalCount / limit);
        return new UsersResponse{users = users, totalPages = totalPages, totalCount = totalCount};
    }

    public async Task DeleteUser(string username)
    {
        var user = GetSingleUserOrBadRequest(username, "Username were not found");
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public User GetSingleUserOrBadRequest(string username, string error)
    {
        var user = _dbContext.Users.SingleOrDefault(x => x.Username == username); //returns a single item.
        if (user == null) throw new BadHttpRequestException(error);
        return user;
    }

    public UserByUsernameRes GetUserByUsername(string username)
    {
        var rawUser = GetSingleUserOrBadRequest(username, "Username is wrong");
        var user = new DefaultUserResponse { username = rawUser.Username };
        var posts =
        (
            from post in _dbContext.Posts
            where post.User.Username == user.username
            select new PostResponse { id = post.Id, title = post.Title, user = user, body = post.Body }
        ).ToArray();
        return new UserByUsernameRes { username = username, posts = posts };
    }
}