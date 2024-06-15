using MediatR;
using PostsApp.Application.Common.Interfaces;

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
        var post = await _unitOfWork
            .Books.GetSingleWhereAsync(book => book.Id.Value == request.Id && book.Author.Id.Value == request.UserId);
        _unitOfWork.Books.Remove(post!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}