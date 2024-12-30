using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Books.Commands.PrivilegedDeleteBook;

internal sealed class PrivilegedDeleteBookCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<PrivilegedDeleteBookCommand>
{
    public async Task Handle(PrivilegedDeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await unitOfWork.Books
            .GetSingleById(request.Id, cancellationToken);
        await unitOfWork.Books.Remove(book!);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}