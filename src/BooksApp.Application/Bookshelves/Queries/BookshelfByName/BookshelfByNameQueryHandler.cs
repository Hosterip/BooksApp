using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfByName;

internal sealed class BookshelfByNameQueryHandler : IRequestHandler<BookshelfByNameQuery, BookshelfResult>
{
    private readonly IMapper _mapster;
    private readonly IUnitOfWork _unitOfWork;

    public BookshelfByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapster)
    {
        _unitOfWork = unitOfWork;
        _mapster = mapster;
    }

    public async Task<BookshelfResult> Handle(BookshelfByNameQuery request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetBookshelfByName(request.Name, request.UserId);
        var bookshelfResult = _mapster.Map<BookshelfResult>(bookshelf!);
        return bookshelfResult;
    }
}