using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Reviews.Commands.CreateReview;
using PostsApp.Application.Reviews.Commands.DeleteReview;
using PostsApp.Application.Reviews.Commands.UpdateReview;
using PostsApp.Application.Reviews.Queries.GetReviews;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Review;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

[Route("Reviews")]
public class ReviewController : Controller
{
    private readonly ISender _sender;

    public ReviewController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("many/{id:guid}")]
    public async Task<IActionResult> GetMany(Guid id, int? page, int? pageSize, CancellationToken cancellationToken)
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

    [HttpPost]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create([FromBodyOrDefault] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        CreateReviewCommand command = new CreateReviewCommand
        {
            BookId = request.BookId,
            UserId = new Guid(HttpContext.GetId()!),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Update([FromBodyOrDefault]UpdateReviewRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand
        {
            ReviewId = request.ReviewId,
            UserId = new Guid(HttpContext.GetId()!),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand
        {
            ReviewId = id,
            UserId = new Guid(HttpContext.GetId()!),
        };
        await _sender.Send(command, cancellationToken);
        return Ok("Review was successfully deleted");
    }
    
    
}