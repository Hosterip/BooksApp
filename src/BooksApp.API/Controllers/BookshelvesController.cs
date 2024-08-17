using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Bookshelves.Commands.AddBook;
using PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;
using PostsApp.Application.Bookshelves.Commands.CreateBookshelf;
using PostsApp.Application.Bookshelves.Commands.RemoveBook;
using PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;
using PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using PostsApp.Application.Bookshelves.Queries.GetBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Bookshelf;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

[Route("[controller]")]
public class BookshelvesController : Controller
{
    private readonly ISender _sender;

    public BookshelvesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetBookshelves(Guid userId)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await _sender.Send(query);

        return Ok(result);
    }
    
    [HttpGet("books/{bookshelfId:guid}")]
    public async Task<IActionResult> GetBooks(Guid bookshelfId, int? limit, int? page)
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
    
    [HttpPost("createBookshelf/{name}")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create(string name)
    {
        var command = new CreateBookshelfCommand
        {
            Name = name,
            UserId = Guid.Parse(HttpContext.GetId()!)
        };
        var result = await _sender.Send(command);

        return Ok(result);
    }
    
    [HttpPost("addBook")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> AddBookToBookshelf(
        [FromBodyOrDefault]AddRemoveBookBookshelfRequest request)
    {
        var command = new AddBookCommand
        {
            BookshelfId = request.BookshelfId,
            BookId = request.BookId,
            UserId = Guid.Parse(HttpContext.GetId()!)
        };
        await _sender.Send(command);

        return Ok("Book was added successfully!");
    }
    
    [HttpPost("addBookToDefaultBookshelf")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> AddBookToDefaultBookshelf(
        [FromBodyOrDefault]AddRemoveBookToDefaultBookshelfRequest request)
    {
        var command = new AddBookToDefaultBookshelfCommand()
        {
            BookshelfName = request.BookshelfName,
            BookId = request.BookId,
            UserId = Guid.Parse(HttpContext.GetId()!)
        };
        await _sender.Send(command);

        return Ok("Book was added successfully!");
    }
    
    [HttpDelete("removeBook")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> RemoveBookBookshelf(
        [FromBodyOrDefault]AddRemoveBookBookshelfRequest request)
    {
        var command = new RemoveBookCommand
        {
            BookshelfId = request.BookshelfId,
            BookId = request.BookId,
            UserId = Guid.Parse(HttpContext.GetId()!)
        };
        await _sender.Send(command);

        return Ok("Book was deleted successfully!");
    }
    
    [HttpDelete("removeBookFromDefaultBookshelf")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> RemoveBookFromDefaultBookshelf(
        [FromBodyOrDefault]AddRemoveBookToDefaultBookshelfRequest request)
    {
        var command = new RemoveBookFromDefaultBookshelfCommand
        {
            BookshelfName = request.BookshelfName,
            BookId = request.BookId,
            UserId = Guid.Parse(HttpContext.GetId()!)
        };
        await _sender.Send(command);

        return Ok("Book was deleted successfully!");
    }
}