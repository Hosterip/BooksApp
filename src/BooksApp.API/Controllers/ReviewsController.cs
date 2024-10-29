using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Reviews.Commands.CreateReview;
using BooksApp.Application.Reviews.Commands.DeleteReview;
using BooksApp.Application.Reviews.Commands.UpdateReview;
using BooksApp.Application.Reviews.Queries.GetReviews;
using BooksApp.Contracts.Requests.Reviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class ReviewsController : ApiController
{
    private readonly ISender _sender;

    public ReviewsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiRoutes.Reviews.Create)]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBodyOrDefault] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateReviewCommand
        {
            BookId = request.BookId,
            UserId = HttpContext.GetId()!.Value,
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut(ApiRoutes.Reviews.Update)]
    [Authorize]
    public async Task<IActionResult> Update(
        [FromBodyOrDefault] UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand
        {
            ReviewId = request.ReviewId,
            UserId = HttpContext.GetId()!.Value,
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete(ApiRoutes.Reviews.Delete)]
    [Authorize]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand
        {
            ReviewId = id,
            UserId = HttpContext.GetId()!.Value
        };
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    // Books

    [HttpGet(ApiRoutes.Books.GetReviews)]
    public async Task<IActionResult> GetReviews(
        Guid id,
        [FromQuery] GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewsQuery
        {
            BookId = id,
            Page = request.Page,
            Limit = request.Limit
        };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}