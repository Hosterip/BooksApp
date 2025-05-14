using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
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
        ValidateRating(rating);
        ValidateBody(body);

        return new Review(
            ReviewId.Create(),
            rating,
            body,
            user,
            book
        );
    }

    public void ChangeRating(int rating)
    {
        ValidateRating(rating);

        Rating = rating;
    }

    public void ChangeBody(string body)
    {
        ValidateBody(body);

        Body = body;
    }

    private static void ValidateRating(int rating)
    {
        if (rating is > 5 or < 1)
            throw new DomainException("Rating should be between 1 to 5 inclusively");
    }

    private static void ValidateBody(string body)
    {
        if (body.Length is > MaxPropertyLength.Review.Body or < 1)
            throw new DomainException($"Body length should be between 1 and {MaxPropertyLength.Review.Body}");
        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Body can not be full of whitespace");
    }
}