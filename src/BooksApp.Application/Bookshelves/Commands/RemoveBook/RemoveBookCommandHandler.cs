using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

internal sealed class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
        var removed = bookshelf!.RemoveBook(request.BookId);
        if (removed) await _unitOfWork.SaveAsync(cancellationToken);
    }
}