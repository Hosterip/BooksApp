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
        var post = await _unitOfWork.Posts.GetSingleWhereAsync(post => post.Id == request.Id);
        post!.Title = request.Title;
        post.Description = request.Body;
        await _unitOfWork.SaveAsync(cancellationToken);
        var likeCount = await _unitOfWork.Likes.CountLikes(request.Id);
        return new BookResult
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            LikeCount = likeCount,
            Author = new UserResult{ Username = post.Author.Username, Id = post.Author.Id }
        };
    }
}