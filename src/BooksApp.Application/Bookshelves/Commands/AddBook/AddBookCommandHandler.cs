using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBook;

internal sealed class AddBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddBookCommand>
{
    public async Task Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
        var book = await unitOfWork.Books.GetSingleById(request.BookId, cancellationToken);
        
        bookshelf!.AddBook(book!);

        await unitOfWork.Bookshelves.Update(bookshelf);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}