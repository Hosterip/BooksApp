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
            .Books.GetSingleWhereAsync(post => post.Id == request.Id && post.Author.Id == request.UserId);
        await _unitOfWork.Books.RemoveAsync(post!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}