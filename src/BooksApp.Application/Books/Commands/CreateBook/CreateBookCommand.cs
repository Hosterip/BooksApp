using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Books.Commands.CreateBook;

[Authorize]
public sealed class CreateBookCommand : IRequest<BookResult>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IFormFile Image { get; init; }
    public required List<Guid> GenreIds { get; init; }
}