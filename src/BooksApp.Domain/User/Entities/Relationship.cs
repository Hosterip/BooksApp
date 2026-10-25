using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain;

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
        return new Relationship(Domain.FollowerId.Create(), user.Id, follower.Id);
    }
}