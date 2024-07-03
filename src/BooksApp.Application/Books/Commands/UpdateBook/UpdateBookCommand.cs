using MediatR;
using Microsoft.AspNetCore.Http;
using PostsApp.Application.Books.Results;

namespace PostsApp.Application.Books.Commands.UpdateBook;

public class UpdateBookCommand : IRequest<BookResult>
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public required string? ImageName { get; init; }
    public required List<Guid> GenreIds { get; init; }
}