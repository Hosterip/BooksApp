using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Bookshelves.Commands.CreateBookshelf;
using PostsApp.Application.Bookshelves.Commands.DeleteBookshelf;
using PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using PostsApp.Application.Bookshelves.Queries.GetBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Extensions;

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
        string name)
    {
        var command = new CreateBookshelfCommand
        {
            Name = name,
            UserId = Guid.Parse(HttpContext.GetId()!)
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
}