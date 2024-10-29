using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Books.Commands.DeleteBook;

internal sealed class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork
            .Books.GetSingleWhereAsync(book =>
                book.Id == BookId.CreateBookId(request.Id) &&
                book.Author.Id == UserId.CreateUserId(request.UserId));
        _unitOfWork.Books.Remove(book!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}