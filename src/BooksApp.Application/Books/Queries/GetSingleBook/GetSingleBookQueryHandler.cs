using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Books.Queries.GetSingleBook;

internal sealed class GetSingleBookQueryHandler : IRequestHandler<GetSingleBookQuery, BookResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleBookQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<BookResult> Handle(GetSingleBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.Id);

        var user = new UserResult
        {
            Id = book!.Author.Id.Value.ToString(), 
            Username = book!.Author.Username,
            Role = book.Author.Role.Name,
            AvatarName = book.Author.Avatar?.ImageName
        };

        var average = _unitOfWork.Books.AverageRating(book.Id.Value);
        
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