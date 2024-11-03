using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelves;

internal sealed class GetBookshelvesQueryHandler : IRequestHandler<GetBookshelvesQuery, IEnumerable<BookshelfResult>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetBookshelvesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookshelfResult>> Handle(GetBookshelvesQuery request, CancellationToken cancellationToken)
    {
        var rawBookshelves = await _unitOfWork.Bookshelves
            .GetAllWhereAsync(bookshelf =>
                bookshelf.User != null && bookshelf.User.Id == UserId.CreateUserId(request.UserId));
        var bookshelves =
            rawBookshelves.Select(bookshelf => _mapper.Map<BookshelfResult>(bookshelf)).ToList();
        return bookshelves;
    }
}