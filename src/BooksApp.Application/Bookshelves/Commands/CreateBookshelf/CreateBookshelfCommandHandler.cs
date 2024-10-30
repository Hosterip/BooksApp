using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

internal sealed class CreateBookshelfCommandHandler : IRequestHandler<CreateBookshelfCommand, BookshelfResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookshelfCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookshelfResult> Handle(CreateBookshelfCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var bookshelf = Bookshelf.Create(user!, request.Name);
        await _unitOfWork.Bookshelves.AddAsync(bookshelf);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<BookshelfResult>(bookshelf);
    }
}