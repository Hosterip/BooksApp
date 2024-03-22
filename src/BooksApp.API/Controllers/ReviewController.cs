using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Reviews.Commands.CreateReview;
using PostsApp.Application.Reviews.Commands.DeleteReview;
using PostsApp.Application.Reviews.Commands.UpdateReview;
using PostsApp.Application.Reviews.Queries.GetReviews;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Review;

namespace PostsApp.Controllers;

[Route("Reviews")]
public class ReviewController : Controller
{
    private readonly ISender _sender;

    public ReviewController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("many/{id:int}")]
    public async Task<IActionResult> GetMany(int id, int? page, int? pageSize, CancellationToken cancellationToken)
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
    public async Task<IActionResult> Create(CreateReviewRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are already authorized");
        CreateReviewCommand command = new CreateReviewCommand
        {
            BookId = request.BookId,
            UserId = HttpContext.GetId(),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateReviewRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are already authorized");
        var command = new UpdateReviewCommand
        {
            ReviewId = request.ReviewId,
            UserId = HttpContext.GetId(),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are already authorized");
        
        var command = new DeleteReviewCommand
        {
            ReviewId = id,
            UserId = HttpContext.GetId(),
        };
        await _sender.Send(command, cancellationToken);
        return Ok("Review was successfully deleted");
    }
    
    
}