using Contractss.Requests.Bookshelves;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Bookshelves.Commands.AddBook;
using PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;
using PostsApp.Application.Bookshelves.Commands.CreateBookshelf;
using PostsApp.Application.Bookshelves.Commands.DeleteBookshelf;
using PostsApp.Application.Bookshelves.Commands.RemoveBook;
using PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;
using PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using PostsApp.Application.Bookshelves.Queries.GetBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class BookshelvesController : ApiController
{
    private readonly ISender _sender;

    public BookshelvesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiRoutes.Bookshelves.GetBooks)]
    public async Task<IActionResult> GetBooks(
        Guid bookshelfId,
        int? limit,
        int? page)
    {
        var query = new GetBookshelfBooksQuery
        {
            BookshelfId = bookshelfId,
            Limit = limit,
            Page = page
        };
        var result = await _sender.Send(query);

        return Ok(result);
    }

    [HttpPost(ApiRoutes.Bookshelves.Create)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create(
        [FromBodyOrDefault] CreateBookshelfRequest request)
    {
        var command = new CreateBookshelfCommand
        {
            Name = request.Name,
            UserId = HttpContext.GetId()!.Value
        };
        var result = await _sender.Send(command);

        return Ok(result);
    }

    [HttpDelete(ApiRoutes.Bookshelves.Remove)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Remove(
        Guid bookshelfId)
    {
        var command = new DeleteBookshelfCommand
        {
            BookshelfId = bookshelfId
        };

        await _sender.Send(command);

        return Ok("Bookshelf deleted");
    }
    
    // Books endpoints
    
    [HttpPost(ApiRoutes.Books.AddBook)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> AddBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName)
    {
        var userId = HttpContext.GetId()!.Value;
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
        var userId = HttpContext.GetId()!.Value;
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

    // Users endpoints 

    [HttpGet(ApiRoutes.Users.GetBookshelves)]
    public async Task<IActionResult> GetBookshelves(
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