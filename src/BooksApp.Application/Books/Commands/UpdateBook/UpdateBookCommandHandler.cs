using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;

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
        if (request.Title is not null)
            book!.Title = request.Title;
        if (request.Description is not null)
            book!.Description = request.Description;
        if (request.ImageName is not null)
        {
            book!.Cover.ImageName = request.ImageName;
        }

        book!.Genres = _unitOfWork.Genres.GetAllByIds(request.GenreIds).ToList()!;

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
            CoverName = book.Cover.ImageName,
            Genres = book.Genres.Select(genre => new GenreResult{ Id = genre.Id.Value, Name = genre.Name }).ToList()
        };
    }
}