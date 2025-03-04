using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Common.Helpers;
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
        MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
        LastName = string.IsNullOrWhiteSpace(lastName) ? null : lastName;
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

    public static User Create(
        IPasswordHasher passwordHasher,
        string email,
        Role.Role role,
        string password,
        Image.Image? avatar,
        string firstName,
        string? middleName = null,
        string? lastName = null)
    {
        ValidateEmail(email);
        ValidateName(firstName, middleName, lastName);
        var (hash, salt) = passwordHasher.GenerateHashSalt(password);

        return new User(
            UserId.Create(),
            email.ToLower(),
            firstName,
            middleName,
            lastName,
            role,
            hash,
            salt,
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

        if (HasFollower(follower.Id))
            throw new DomainException("There is already a follower");

        var item = Relationship.Create(this, follower);
        _followers.Add(item);
    }

    public void RemoveFollower(User follower)
    {
        if (follower.Id == Id)
            throw new DomainException("Can't add yourself to followers");

        if (!HasFollower(follower.Id))
            throw new DomainException("No follower to remove");

        _followers.RemoveAll(x => x.FollowerId == follower.Id);
    }

    public bool HasFollower(UserId followerId)
    {
        return _followers.Any(x => x.FollowerId == followerId);
    }

    public (bool IsFollowing, bool IsFriend, bool IsMe) ViewerRelationship(Guid? followerId)
    {
        if (followerId == null) return (false, false, false);
        if (followerId.Value == Id.Value)
            return (false, false, true);
        var userId = UserId.Create(followerId);
        var isFollowing = _followers.Any(x => x.FollowerId == userId);
        var isFriend = isFollowing && _following.Any(x => x.UserId == userId);
        return (isFollowing, isFriend, false);
    }

    private static void ValidateEmail(string email)
    {
        if (!EmailValidator.Validate(email))
            throw new DomainException("Invalid email");
    }

    private static void ValidateName(string firstName, string? middleName, string? lastName)
    {
        if (firstName.Length is > MaxPropertyLength.User.FirstName or < 1)
            throw new DomainException($"First name length should be between 1 and {MaxPropertyLength.User.FirstName}");
        if (middleName?.Length is > MaxPropertyLength.User.MiddleName or < 1)
            throw new DomainException($"Middle name length should be between 1 and {MaxPropertyLength.User.MiddleName}");
        if (lastName?.Length is > MaxPropertyLength.User.LastName or < 1)
            throw new DomainException($"Last name length should be between 1 and {MaxPropertyLength.User.LastName}");
        
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name should be present");
    }
}