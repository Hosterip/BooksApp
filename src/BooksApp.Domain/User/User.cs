using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Image.ValueObjects;
using PostsApp.Domain.Role.ValueObjects;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Domain.User;

public class User : AggregateRoot<UserId>
{
    private User(UserId id) : base(id) { }

    private User(UserId id, string email, string firstName, string? middleName, string? lastName, Role.Role role, string hash, string salt, string securityStamp, Image.Image? avatar) : base(id)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Role = role;
        Hash = hash;
        Salt = salt;
        SecurityStamp = securityStamp;
        Avatar = avatar;
    }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public Role.Role Role { get; set; }
    public string Hash { get; set; }
    public string Salt { get; set; }
    public string SecurityStamp { get; set; }
    public Image.Image? Avatar { get; set; }

    public static User Create(string email, string firstName, string? middleName, string? lastName, Role.Role role, string hash, string salt, Image.Image? avatar)
    {
        return new(UserId.CreateUserId(), email.ToLower(), firstName, middleName, lastName, role, hash, salt, Guid.NewGuid().ToString(), avatar);
    }
}