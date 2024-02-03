using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Posts.Commands.AddRemoveLike;
using PostsApp.Application.Posts.Commands.CreatePost;
using PostsApp.Application.Posts.Commands.DeletePost;
using PostsApp.Application.Posts.Commands.UpdatePost;
using PostsApp.Application.Posts.Queries.GetPosts;
using PostsApp.Application.Posts.Queries.GetSinglePost;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Post;

namespace PostsApp.Controllers;

[Route("posts")]
public class PostController : Controller
{
    private readonly ISender _sender;

    public PostController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(PostRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to make post");


        var command = new CreatePostCommand
        {
            Id = (int)HttpContext.GetId()!, Title = request.Title, Body = request.Body
        };
        var post = await _sender.Send(command, cancellationToken);

        return StatusCode(201, post);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePost(UpdatePostRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");

        var command = new UpdatePostCommand
        {
            Id = request.Id, UserId = (int)HttpContext.GetId()!, Title = request.Title, Body = request.Body
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to delete post");

        var command = new DeletePostCommand { Id = id, UserId = (int)HttpContext.GetId()! };
        await _sender.Send(command, cancellationToken);
        return Ok("Post was deleted");
    }

    [HttpGet("many")]
    public async Task<IActionResult> GetPosts(int? page, int? limit, string q, CancellationToken cancellationToken)
    {
        var query = new GetPostsQuery { Query = q, Limit = limit, Page = page };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("single/{id:int}")]
    public async Task<IActionResult> GetPost(int id, CancellationToken cancellationToken)
    {
        var query = new GetSinglePostQuery { Id = id };
        var post = await _sender.Send(query, cancellationToken);
        return Ok(post);
    }

    [HttpPost("like/{id:int}")]
    public async Task<IActionResult> AddRemoveLike(int id, CancellationToken cancellationToken)
    {
        var query = new AddRemoveLikeCommand { UserId = (int)HttpContext.GetId()!, PostId = id };
        await _sender.Send(query, cancellationToken);
        return Ok("Like was added or removed");
    }
}