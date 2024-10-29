using MediatR;

namespace BooksApp.Application.Books.Commands.DeleteBook;

public class DeleteBookCommand : IRequest
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
}