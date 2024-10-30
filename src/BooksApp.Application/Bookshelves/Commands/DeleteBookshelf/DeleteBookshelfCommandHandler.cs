using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;

internal sealed class DeleteBookshelfCommandHandler : IRequestHandler<DeleteBookshelfCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookshelfCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteBookshelfCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleById(request.BookshelfId);
        _unitOfWork.Bookshelves.Remove(bookshelf!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}