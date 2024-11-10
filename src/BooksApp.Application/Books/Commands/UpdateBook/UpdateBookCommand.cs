using BooksApp.Application.Books.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Books.Commands.UpdateBook;

public class UpdateBookCommand : IRequest<BookResult>
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string? Title { get; init; }
    public required string? Description { get; init; }
    public required IFormFile? Image { get; init; }
    public required List<Guid> GenreIds { get; init; }
}