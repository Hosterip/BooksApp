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

    public int Rating { get; set; }
    public string Body { get; set; }
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
}