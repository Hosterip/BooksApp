using BooksApp.Domain.Book;
using BooksApp.Domain.Review;
using BooksApp.Domain.User;
using TestCommon.Books;
using TestCommon.Common.Constants;
using TestCommon.Users;

namespace TestCommon.Reviews;

public static class ReviewFactory
{
    public static Review CreateReview(
        int rating = Constants.Reviews.Rating,
        string body = Constants.Reviews.Body,
        User? user = null,
        Book? book = null)
    {
        return Review.Create(
            rating,
            body,
            user ?? UserFactory.CreateUser(),
            book ?? BookFactory.CreateBook());
    }
}