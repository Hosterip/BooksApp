using BooksApp.Domain.Common.Models;
using BooksApp.Domain.User.ValueObjects;

namespace BooksApp.Domain.User.Entities;

public class Relationship : Entity<FollowerId>
{
    public UserId FollowerId { get; private set; }
    public UserId UserId { get; private set; }
    
    private Relationship(FollowerId id) : base(id) {}

    private Relationship(FollowerId id, UserId userId, UserId follower) : base(id)
    {
        FollowerId = follower;
        UserId = userId;
    }

    public static Relationship Create(User user,User follower)
    {
        return new(ValueObjects.FollowerId.Create(), user.Id, follower.Id);
    }
}