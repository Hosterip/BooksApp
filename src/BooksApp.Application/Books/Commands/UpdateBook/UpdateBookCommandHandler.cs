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
        var book = await _unitOfWork.Books.GetSingleById(request.Id);
        book!.Title = request.Title;
        book.Description = request.Body;
        if (request.ImageName is not null)
        {
            book.Cover.ImageName = request.ImageName;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        var average = _unitOfWork.Books.AverageRating(book.Id.Value);

        var user = new UserResult
        {
            Id = book.Author.Id.Value.ToString(),
            Username = book.Author.Username,
            Role = book.Author.Role.Name,
            AvatarName = book.Author.Avatar?.ImageName
        };
        return new BookResult
        {
            Id = book.Id.Value.ToString(),
            Title = book.Title,
            Description = book.Description,
            Average = average,
            Author = user,
            CoverName = book.Cover.ImageName
        };
    }
}