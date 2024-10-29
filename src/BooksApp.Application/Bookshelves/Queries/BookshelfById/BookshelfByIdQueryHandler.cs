using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfById;

public class BookshelfByNameQueryHandler : IRequestHandler<BookshelfByIdQuery, BookshelfResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapster;
    
    public BookshelfByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapster)
    {
        _unitOfWork = unitOfWork;
        _mapster = mapster;
    }

    public async Task<BookshelfResult> Handle(BookshelfByIdQuery request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleById(request.BookshelfId);
        var bookshelfResult = _mapster.Map<BookshelfResult>(bookshelf!);
        return bookshelfResult;
    }
}