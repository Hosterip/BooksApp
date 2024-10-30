using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Bookshelves;
using BooksApp.Application.Bookshelves.Commands.AddBook;
using BooksApp.Application.Bookshelves.Commands.AddBookByName;
using BooksApp.Application.Bookshelves.Commands.CreateBookshelf;
using BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;
using BooksApp.Application.Bookshelves.Commands.RemoveBook;
using BooksApp.Application.Bookshelves.Commands.RemoveBookByName;
using BooksApp.Application.Bookshelves.Queries.BookshelfById;
using BooksApp.Application.Bookshelves.Queries.BookshelfByRefName;
using BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using BooksApp.Application.Bookshelves.Queries.GetBookshelves;
using BooksApp.Application.Common.Results;
using BooksApp.Contracts.Requests.Bookshelves;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class BookshelvesController : ApiController
{
    private readonly ISender _sender;

    public BookshelvesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiRoutes.Users.GetBookshelf)]
    public async Task<ActionResult<BookshelfResult>> GetBookshelf(
        [FromRoute] string nameOrGuid,
        [FromRoute] Guid userId
    )
    {
        var success = Guid.TryParse(nameOrGuid, out var bookshelfId);
        var result = await _sender.Send(success
            ? new BookshelfByIdQuery
            {
                BookshelfId = bookshelfId
            }
            : new BookshelfByNameQuery
            {
                UserId = userId,
                Name = nameOrGuid
            }
        );
        return Ok(result);
    }

    [HttpGet(ApiRoutes.Bookshelves.GetBooks)]
    public async Task<ActionResult<PaginatedArray<BookResult>>> GetBooks(
        [FromRoute] Guid bookshelfId,
        [FromQuery] GetBookshelfBooksRequest request)
    {
        var query = new GetBookshelfBooksQuery
        {
            BookshelfId = bookshelfId,
            Limit = request.Limit,
            Page = request.Page
        };
        var result = await _sender.Send(query);

        return Ok(result);
    }

    [HttpPost(ApiRoutes.Bookshelves.Create)]
    [Authorize]
    public async Task<ActionResult<BookshelfResult>> Create(
        [FromBodyOrDefault] CreateBookshelfRequest request)
    {
        var userId = HttpContext.GetId()!.Value;
        var command = new CreateBookshelfCommand
        {
            Name = request.Name,
            UserId = userId
        };
        var result = await _sender.Send(command);

        return CreatedAtAction(
            nameof(GetBookshelf),
            new {nameOrGuid = result.Id, userId },
            result);
    }

    [HttpDelete(ApiRoutes.Bookshelves.Remove)]
    [Authorize]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid bookshelfId)
    {
        var userId = HttpContext.GetId();
        var command = new DeleteBookshelfCommand
        {
            BookshelfId = bookshelfId,
            UserId = userId!.Value
        };

        await _sender.Send(command);

        return Ok();
    }

    // Books endpoints

    [HttpPost(ApiRoutes.Books.AddBook)]
    [Authorize]
    public async Task<IActionResult> AddBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName)
    {
        var userId = HttpContext.GetId()!.Value;
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        await _sender.Send(success
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

        return Ok();
    }

    [HttpDelete(ApiRoutes.Books.RemoveBook)]
    [Authorize]
    public async Task<IActionResult> RemoveBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName)
    {
        var userId = HttpContext.GetId()!.Value;
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        await _sender.Send(success 
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
        return Ok();
    }

    // Users endpoints 

    [HttpGet(ApiRoutes.Users.GetBookshelves)]
    public async Task<ActionResult<List<BookshelfResult>>> GetBookshelves(
        Guid userId)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await _sender.Send(query);

        return Ok(result);
    }
}