using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Commands.UpdateBook;

internal sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<BookResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleWhereAsync(book => book.Id == request.Id);
        book!.Title = request.Title;
        book.Description = request.Body;
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var average = _unitOfWork.Books.AverageRating(book.Id);
        
        var user = new UserResult
        {
            Id = book.Author.Id,
            Username = book.Author.Username,
            Role = book.Author.Role.Name
        };
        return new BookResult
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Average = average,
            Author = user
        };
    }
}