using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Books.Commands.DeleteBook;

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