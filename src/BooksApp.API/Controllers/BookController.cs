using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Commands.DeleteBook;
using PostsApp.Application.Books.Commands.UpdateBook;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Books.Queries.GetSingleBook;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Book;
using PostsApp.Contracts.Requests.Post;

namespace PostsApp.Controllers;

[Route("books")]
public class BookController : Controller
{
    private readonly ISender _sender;

    public BookController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]BookRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to create a book");


        var command = new CreateBookCommand
        {
            UserId = HttpContext.GetId(), Title = request.Title, Description = request.Description
        };
        var post = await _sender.Send(command, cancellationToken);

        return StatusCode(201, post);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody]UpdateBookRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");

        var command = new UpdateBookCommand
        {
            Id = request.Id, UserId = (int)HttpContext.GetId()!, Title = request.Title, Body = request.Description
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to delete a book");

        var command = new DeleteBookCommand { Id = id, UserId = (int)HttpContext.GetId()! };
        await _sender.Send(command, cancellationToken);
        return Ok("Book was deleted");
    }

    [HttpGet("many")]
    public async Task<IActionResult> GetMany(int? page, int? limit, string q, CancellationToken cancellationToken)
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("single/{id:int}")]
    public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
    {
        var query = new GetSingleBookQuery { Id = id };
        var post = await _sender.Send(query, cancellationToken);
        return Ok(post);
    }
}