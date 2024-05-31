using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

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
        var book = await _unitOfWork.Books.GetSingleWhereAsync(post => post.Id == request.Id);

        var user = new UserResult
        {
            Id = book!.Author.Id, 
            Username = book!.Author.Username,
            Role = book.Author.Role.Name,
            AvatarName = book.Author.Avatar?.ImageName
        };

        var average = _unitOfWork.Books.AverageRating(book.Id);
        
        return new BookResult
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Average = average,
            Author = user,
            CoverName = book.Cover.ImageName
        };
    }
}