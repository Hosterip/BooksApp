using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Books.Commands.DeleteBook;
using BooksApp.Application.Books.Commands.UpdateBook;
using BooksApp.Application.Books.Queries.GetBooks;
using BooksApp.Application.Books.Queries.GetSingleBook;
using BooksApp.Application.Books.Results;
using BooksApp.Contracts.Requests.Books;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

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
        [FromQuery] GetBooksRequest request
    )
    {
        var query = new GetBooksQuery
        {
            Query = request.Q,
            Limit = request.Limit,
            Page = request.Page,
            GenreId = request.GenreId,
            UserId = null
        };
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
    [Authorize]
    public async Task<ActionResult<BookResult>> Create(
        [FromBodyOrDefault] CreateBookRequest request,
        CancellationToken cancellationToken
    )
    {
        var createBookCommand = new CreateBookCommand
        {
            UserId = HttpContext.GetId()!.Value,
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        var book = await _sender.Send(createBookCommand, cancellationToken);

        return CreatedAtAction(
            nameof(GetSingle),
            new {id = book.Id}, book);
    }

    [HttpPut(ApiRoutes.Books.Update)]
    [Authorize]
    public async Task<ActionResult<BookResult>> Update(
        [FromBodyOrDefault] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var updateBookCommand = new UpdateBookCommand
        {
            Id = request.Id,
            UserId = HttpContext.GetId()!.Value,
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        var result = await _sender.Send(updateBookCommand, cancellationToken);
        return CreatedAtAction(
            nameof(GetSingle),
            new {id = result.Id}, result);
    }

    [HttpDelete(ApiRoutes.Books.Delete)]
    [Authorize]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = id, UserId = HttpContext.GetId()!.Value };
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    // Users endpoints

    [HttpGet(ApiRoutes.Users.GetManyBooks)]
    public async Task<IActionResult> GetManyByUserId(
        CancellationToken cancellationToken,
        [FromRoute] Guid userId,
        [FromQuery] GetUserBooksRequest request
    )
    {
        var query = new GetBooksQuery
        {
            Query = request.Q,
            Limit = request.Limit,
            Page = request.Page,
            GenreId = request.GenreId,
            UserId = userId
        };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}