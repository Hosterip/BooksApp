using MediatR;

namespace PostsApp.Application.Books.Commands.DeleteBook;

public class DeleteBookCommand : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
}