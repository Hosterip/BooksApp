using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Book;
using PostsApp.Domain.Image;

namespace PostsApp.Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<BookResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var image = Image.Create(request.ImageName);
        var book = Book.Create(request.Title, request.Description, image, user!);
        await _unitOfWork.Images.AddAsync(image);
        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveAsync(cancellationToken);
        var result = new BookResult
        {
            Id = book.Id.Value.ToString(),
            Author = new UserResult
            {
                Id = user!.Id.Value.ToString(),
                Username = user.Username,
                Role = user.Role.Name,
                AvatarName = user.Avatar?.ImageName 
            },
            Title = book.Title, Description = book.Description,
            Average = 0,
            CoverName = book.Cover.ImageName
        };
        return result;
    }
}