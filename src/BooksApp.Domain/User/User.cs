using System.ComponentModel.DataAnnotations;
using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
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
    public string FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string? LastName { get; private set; }
    public Role.Role Role { get; private set; }
    private string Hash { get; set; }
    private string Salt { get; set; }
    public string SecurityStamp { get; private set; }
    public Image.Image? Avatar { get; set; }
    private readonly List<Relationship> _followers = [];
    public IReadOnlyList<Relationship> Followers => _following.AsReadOnly();
    private readonly List<Relationship> _following = [];
    public IReadOnlyList<Relationship> Following => _following.AsReadOnly();

    public static User Create(string email, string firstName, string? middleName, string? lastName, Role.Role role,
        string hash, string salt, Image.Image? avatar)
    {
        ValidateEmail(email);
        ValidateName(firstName, middleName, lastName);
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

    public void ChangeName(string firstName, string? middleName, string? lastName)
    {
        ValidateName(firstName, middleName, lastName);
        
        FirstName = firstName;
        
        MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
        LastName = string.IsNullOrWhiteSpace(lastName) ? null : lastName;
    }

    public void ChangeRole(Role.Role role)
    {
        Role = role;
    }

    public void ChangeSecurityStamp()
    {
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public void AddFollower(User follower)
    {
        if (follower.Id == Id)
            throw new DomainException("Can't add yourself to followers");

        var item = Relationship.Create(this, follower);
        _followers.Add(item);
    }
    
    public void RemoveFollower(User follower)
    {
        if (follower.Id == Id)
            throw new DomainException("Can't add yourself to followers");

        if (_followers.Any(f => f.FollowerId != follower.Id))
            throw new DomainException("No follower to remove");
        
        _followers.RemoveAll(x => x.FollowerId == follower.Id);
    }

    public bool HasFollower(UserId userId)
    {
        return _followers.Any(x => x.UserId == userId);
    }

    public (bool IsFollowing, bool IsFriend, bool IsMe) ViewerRelationship(Guid? followerId)
    {
        if (followerId == null) return (false, false, false);
        var userId = UserId.CreateUserId(followerId);
        var isMe = Id == userId;
        var isFollowing = !isMe && _followers.Any(x => x.FollowerId == userId);
        var isFriend = !isMe && isFollowing && _following.Any(x => x.UserId == userId);
        return (isFollowing, isFriend, isMe);
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            !new EmailAddressAttribute().IsValid(email))
            throw new DomainException("Invalid email");
    }

    private static void ValidateName(string firstName, string? middleName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name should be present");
        
        if (firstName.Length is > UserMaxLengths.FirstName or < 1)
            throw new DomainException($"First name length should be between 1 and {UserMaxLengths.FirstName}");
        
        if (middleName?.Length is > UserMaxLengths.MiddleName or < 1)
            throw new DomainException($"Middle name length should be between 1 and {UserMaxLengths.MiddleName}");
        
        if (lastName?.Length is > UserMaxLengths.LastName or < 1)
            throw new DomainException($"Last name length should be between 1 and {UserMaxLengths.LastName}");
    }
}