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
        var post = await _unitOfWork.Books.GetSingleWhereAsync(post => post.Id == request.Id);
        post!.Title = request.Title;
        post.Description = request.Body;
        await _unitOfWork.SaveAsync(cancellationToken);
        var likeCount = await _unitOfWork.Likes.CountLikes(request.Id);
        var user = new UserResult
        {
            Id = post.Author.Id,
            Username = post.Author.Username,
            Role = post.Author.Role.Name
        };
        return new BookResult
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            LikeCount = likeCount,
            Author = user
        };
    }
}