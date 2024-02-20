using MediatR;

namespace PostsApp.Application.Books.Commands.DeleteBook;

public class DeleteBookCommand : IRequest
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
}