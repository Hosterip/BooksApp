using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Commands.DeleteBook;
using PostsApp.Application.Books.Commands.UpdateBook;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Books.Queries.GetSingleBook;
using PostsApp.Common.Constants;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Book;
using PostsApp.Contracts.Requests.Post;
using Toycloud.AspNetCore.Mvc.ModelBinding;

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
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create([FromBodyOrDefault]BookRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateBookCommand
        {
            UserId = HttpContext.GetId(), Title = request.Title, Description = request.Description
        };
        var post = await _sender.Send(command, cancellationToken);

        return StatusCode(201, post);
    }

    [HttpPut]
    [Authorize(Policy = Policies.Authorized)]

    public async Task<IActionResult> Update([FromBodyOrDefault]UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBookCommand
        {
            Id = request.Id, UserId = (int)HttpContext.GetId()!, Title = request.Title, Body = request.Description
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = id, UserId = (int)HttpContext.GetId()! };
        await _sender.Send(command, cancellationToken);
        return Ok("Book was deleted");
    }

    [HttpGet("many")]
    public async Task<IActionResult> GetMany(
        CancellationToken cancellationToken,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] int? userId
        )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = userId };
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