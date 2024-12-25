using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Review.ValueObjects;

namespace BooksApp.Domain.Review;

public class Review : Entity<ReviewId>
{
    private Review(ReviewId id) : base(id)
    {
    }

    private Review(
        ReviewId id,
        int rating,
        string body,
        User.User user,
        Book.Book book
    ) : base(id)
    {
        Rating = rating;
        Body = body;
        User = user;
        Book = book;
    }

    public int Rating { get; private set; }
    public string Body { get; private set; }
    public User.User User { get; }
    public Book.Book Book { get; }

    public static Review Create(
        int rating,
        string body,
        User.User user,
        Book.Book book
    )
    {
        return new Review(
            ReviewId.CreateReviewId(),
            rating,
            body,
            user,
            book
        );
    }

    public void ChangeRating(int rating)
    {
        if (rating is > 5 or < 1)
            throw new DomainException("Rating should be between 1 to 5 inclusively");
        
        Rating = rating;
    }

    public void ChangeBody(string body)
    {
        Body = body;
    }
}