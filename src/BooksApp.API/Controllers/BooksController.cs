using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Commands.DeleteBook;
using PostsApp.Application.Books.Commands.UpdateBook;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Books.Queries.GetSingleBook;
using PostsApp.Application.Bookshelves.Commands.AddBook;
using PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;
using PostsApp.Application.Bookshelves.Commands.RemoveBook;
using PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;
using PostsApp.Application.Reviews.Queries.GetReviews;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Book;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class BooksController : ApiController
{
    private readonly ISender _sender;

    public BooksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiRoutes.Books.GetMany)]
    public async Task<IActionResult> GetMany(
        CancellationToken cancellationToken,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] Guid? genreId
    )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = null, GenreId = genreId };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet(ApiRoutes.Books.GetSingle)]
    public async Task<IActionResult> GetSingle(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleBookQuery { Id = id };
        var book = await _sender.Send(query, cancellationToken);
        return Ok(book);
    }

    [HttpPost(ApiRoutes.Books.Create)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create(
        [FromBodyOrDefault] CreateBookRequest request,
        CancellationToken cancellationToken
    )
    {
        var createBookCommand = new CreateBookCommand
        {
            UserId = new Guid(HttpContext.GetId()!),
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        var book = await _sender.Send(createBookCommand, cancellationToken);

        return Ok(book);
    }

    [HttpPut(ApiRoutes.Books.Update)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Update(
        [FromBodyOrDefault] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var updateBookCommand = new UpdateBookCommand
        {
            Id = request.Id,
            UserId = new Guid(HttpContext.GetId()!),
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        var result = await _sender.Send(updateBookCommand, cancellationToken);
        return Ok(result);
    }

    [HttpDelete(ApiRoutes.Books.Delete)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = id, UserId = new Guid(HttpContext.GetId()!) };
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    // Bookshelves logic

    [HttpPost(ApiRoutes.Books.AddBook)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> AddBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName)
    {
        var userId = Guid.Parse(HttpContext.GetId()!);
        Guid.TryParse(idOrName, out var bookshelfId);
        await _sender.Send(bookshelfId != null
            ? new AddBookCommand
            {
                BookshelfId = bookshelfId,
                BookId = bookId,
                UserId = userId
            }
            : new AddBookByNameCommand
            {
                BookshelfName = idOrName,
                BookId = bookId,
                UserId = userId
            }
        );

        return Ok("Book was added successfully!");
    }

    [HttpDelete(ApiRoutes.Books.RemoveBook)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> RemoveBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName)
    {
        var userId = Guid.Parse(HttpContext.GetId()!);
        Guid.TryParse(idOrName, out var bookshelfId);
        await _sender.Send(bookshelfId != null
            ? new RemoveBookCommand
            {
                BookshelfId = bookshelfId,
                BookId = bookId,
                UserId = userId
            }
            : new RemoveBookByNameCommand
            {
                BookshelfName = idOrName,
                BookId = bookId,
                UserId = userId
            }
        );
        return Ok("Book was deleted successfully!");
    }

    // Reviews

    [HttpGet(ApiRoutes.Books.GetReviews)]
    public async Task<IActionResult> GetReviews(
        Guid id,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewsQuery
        {
            BookId = id,
            Page = page ?? 1,
            PageSize = pageSize ?? 10
        };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}