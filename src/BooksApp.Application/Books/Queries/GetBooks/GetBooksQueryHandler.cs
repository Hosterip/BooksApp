using System.Linq.Expressions;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Domain.Book;
using BooksApp.Domain.Genre.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Books.Queries.GetBooks;

internal sealed class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PaginatedArray<BookResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBooksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<BookResult>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var query = request.Query ?? "";
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        var result = await _unitOfWork.Books
            .GetPaginated(request.CurrentUserId, limit, page, BookExpression(query, request.UserId, request.GenreId));


        return result;
    }

    private Expression<Func<Book, bool>> BookExpression(string query, Guid? userId, Guid? genreId)
    {
        return book => book.Title.Contains(query)
                       && (userId == null || book.Author.Id == UserId.CreateUserId(userId))
                       && (genreId == null || book.Genres.Any(genre => genre.Id == GenreId.CreateGenreId(genreId)));
    }
}