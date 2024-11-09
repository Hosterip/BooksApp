using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBook;

internal sealed class AddBookCommandHandler : IRequestHandler<AddBookCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
        var book = await _unitOfWork.Books.GetSingleById(request.BookId, cancellationToken);
        bookshelf!.AddBook(book!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}