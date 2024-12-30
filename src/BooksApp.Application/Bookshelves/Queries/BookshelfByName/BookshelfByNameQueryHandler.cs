using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfByName;

internal sealed class BookshelfByNameQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapster)
    : IRequestHandler<BookshelfByNameQuery, BookshelfResult>
{
    public async Task<BookshelfResult> Handle(BookshelfByNameQuery request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves.GetBookshelfByName(request.Name, request.UserId, cancellationToken);
        var bookshelfResult = mapster.Map<BookshelfResult>(bookshelf!);
        return bookshelfResult;
    }
}