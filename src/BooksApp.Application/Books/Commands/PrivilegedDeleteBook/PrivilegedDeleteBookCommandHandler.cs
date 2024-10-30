using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Books.Commands.PrivilegedDeleteBook;

internal sealed class PrivilegedDeleteBookCommandHandler : IRequestHandler<PrivilegedDeleteBookCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PrivilegedDeleteBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PrivilegedDeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork
            .Books.GetSingleById(request.Id);
        _unitOfWork.Books.Remove(book!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}