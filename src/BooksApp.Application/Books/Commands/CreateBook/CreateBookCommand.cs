using MediatR;
using PostsApp.Application.Books.Results;

namespace PostsApp.Application.Books.Commands.CreateBook;

public sealed class CreateBookCommand : IRequest<BookResult>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}