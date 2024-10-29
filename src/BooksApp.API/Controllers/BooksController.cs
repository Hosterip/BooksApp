using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Commands.DeleteBook;
using PostsApp.Application.Books.Commands.UpdateBook;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Books.Queries.GetSingleBook;
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
            UserId = HttpContext.GetId()!.Value,
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
            UserId = HttpContext.GetId()!.Value,
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
        var command = new DeleteBookCommand { Id = id, UserId = HttpContext.GetId()!.Value };
        await _sender.Send(command, cancellationToken);
        return Ok();
    }
    
    // Users endpoints
    
    [HttpGet(ApiRoutes.Users.GetManyBooks)]
    public async Task<IActionResult> GetManyByUserId(
        CancellationToken cancellationToken,
        [FromRoute] Guid userId,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] Guid? genreId
    )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = userId, GenreId = genreId };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}