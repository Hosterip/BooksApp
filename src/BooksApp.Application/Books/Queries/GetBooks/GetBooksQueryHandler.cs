using System.Linq.Expressions;
using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Book;
using PostsApp.Domain.Genre.ValueObjects;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Books.Queries.GetBooks;

internal sealed class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PaginatedArray<BookResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBooksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<BookResult>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        string query = request.Query ?? "";
        int limit = request.Limit ?? 10;
        int page = request.Page ?? 1;

        var result = await _unitOfWork.Books
            .GetPaginated(limit, page, BookExpression(query, request.UserId, request.GenreId));


        return result;
    }

    private Expression<Func<Book, bool>> BookExpression(string query, Guid? userId, Guid? genreId)
    {
        return book => book.Title.Contains(query)
                       && (userId == null || book.Author.Id == UserId.CreateUserId(userId))
                       && (genreId == null || book.Genres.Any(genre => genre.Id == GenreId.CreateGenreId(genreId)));
    }
}