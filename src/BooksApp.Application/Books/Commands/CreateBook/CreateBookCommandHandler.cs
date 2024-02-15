using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;

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
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.Id);
        var post = new Book { Author = user!, Title = request.Title, Description = request.Description };
        await _unitOfWork.Posts.AddAsync(post);
        await _unitOfWork.SaveAsync(cancellationToken);
        var result = new BookResult
        {
            Author = new UserResult { Id = user!.Id, Username = user.Username, Role = user.Role.Name },
            Title = post.Title, Description = post.Description,
            LikeCount = 0,
            Id = post.Id
        };
        return result;
    }
}