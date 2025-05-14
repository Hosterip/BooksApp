using BooksApp.Domain.Common.Models;
using BooksApp.Domain.User.ValueObjects;

namespace BooksApp.Domain.User.Entities;

public class Relationship : Entity<FollowerId>
{
    private Relationship(FollowerId id) : base(id)
    {
    }

    private Relationship(FollowerId id, UserId userId, UserId follower) : base(id)
    {
        FollowerId = follower;
        UserId = userId;
    }

    public UserId FollowerId { get; private set; }
    public UserId UserId { get; private set; }

    public static Relationship Create(User user, User follower)
    {
        return new Relationship(ValueObjects.FollowerId.Create(), user.Id, follower.Id);
    }
}