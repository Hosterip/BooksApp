using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Books.Commands.DeleteBook;

internal sealed class DeleteBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBookCommand>
{
    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await unitOfWork.Books
            .GetSingleById(request.Id, cancellationToken);
        await unitOfWork.Books.Remove(book!);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}