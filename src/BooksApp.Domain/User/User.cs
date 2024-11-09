using System.ComponentModel.DataAnnotations;
using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Interfaces;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.User.Entities;
using BooksApp.Domain.User.ValueObjects;

namespace BooksApp.Domain.User;

public class User : AggregateRoot<UserId>
{
    private User(UserId id) : base(id)
    {
    }

    private User(UserId id, string email, string firstName, string? middleName, string? lastName, Role.Role role,
        string hash, string salt, string securityStamp, Image.Image? avatar) : base(id)
    {
        Email = email;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Role = role;
        Hash = hash;
        Salt = salt;
        SecurityStamp = securityStamp;
        Avatar = avatar;
    }

    public string Email { get; private set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public Role.Role Role { get; set; }
    private string Hash { get; set; }
    private string Salt { get; set; }
    public string SecurityStamp { get; set; }
    public Image.Image? Avatar { get; set; }
    public List<Relationship> Followers { get; private set; } = [];
    public List<Relationship> Following { get; private set; } = [];

    public static User Create(string email, string firstName, string? middleName, string? lastName, Role.Role role,
        string hash, string salt, Image.Image? avatar)
    {
        ValidateEmail(email);
        return new User(UserId.CreateUserId(), email.ToLower(), firstName, middleName, lastName, role, hash, salt,
            Guid.NewGuid().ToString(), avatar);
    }

    public bool IsPasswordValid(IPasswordHasher hasher, string password)
    {
        return hasher.IsPasswordValid(Hash, Salt, password);
    }

    public void ChangePassword(IPasswordHasher hasher, string password)
    {
        var (hash, salt) = hasher.GenerateHashSalt(password);

        Hash = hash;
        Salt = salt;
    }
    
    public void ChangeEmail(string email)
    {
        ValidateEmail(email);
        Email = email;
    }

    public void AddFollower(User follower)
    {
        if (follower.Id == Id)
            throw new DomainException("Can't add yourself to followers");

        var item = Relationship.Create(this, follower);
        Followers.Add(item);
    }
    
    public void RemoveFollower(User follower)
    {
        if (follower.Id == Id)
            throw new DomainException("Can't add yourself to followers");

        if (Followers.Any(f => f.FollowerId != follower.Id))
            throw new DomainException("No follower to remove");
        
        Followers.RemoveAll(x => x.FollowerId == follower.Id);
    }


    public (bool IsFollowing, bool IsFriend, bool IsMe) ViewerRelationship(Guid? followerId)
    {
        if (followerId == null) return (false, false, false);
        var userId = UserId.CreateUserId(followerId);
        var isMe = Id == userId;
        var isFollowing = !isMe && Followers.Any(x => x.FollowerId == userId);
        var isFriend = !isMe && isFollowing && Following.Any(x => x.UserId == userId);
        return (isFollowing, isFriend, isMe);
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            !new EmailAddressAttribute().IsValid(email))
            throw new DomainException("Invalid email");
    }
}