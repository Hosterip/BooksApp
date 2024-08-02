using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Bookshelves.Commands.AddBook;

internal sealed class AddBookCommandHandler : IRequestHandler<AddBookCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleById(request.BookshelfId);
        var book = await _unitOfWork.Books.GetSingleById(request.BookId);
        bookshelf!.Add(book!);
    }
}