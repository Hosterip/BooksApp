using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Posts.Commands.CreatePost;
using PostsApp.Application.Posts.Commands.DeletePost;
using PostsApp.Application.Posts.Commands.UpdatePost;
using PostsApp.Application.Posts.Queries.GetPosts;
using PostsApp.Application.Posts.Queries.GetSinglePost;
using PostsApp.Application.Posts.Results;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Post;
using PostsApp.Domain.Exceptions;

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

        try
        {
            var command = new CreatePostCommand
            {
                Id = (int)HttpContext.GetId()!, Title = request.Title, Body = request.Body
            };
            var post = await _sender.Send(command, cancellationToken);

            return StatusCode(201, post);
        }
        catch (PostException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePost(UpdatePostRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");
        
        try
        {
            var command = new UpdatePostCommand
            {
                Id = request.Id, UserId = (int)HttpContext.GetId()!,Title = request.Title, Body = request.Body
            };
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (PostException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to delete post");

        try
        {
            var command = new DeletePostCommand { Id = id, UserId = (int)HttpContext.GetId()! };
            await _sender.Send(command, cancellationToken);
            return Ok("Post was deleted");
        }
        catch (PostException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("many/{page:int}")]
    public async Task<IActionResult> GetPosts(int page, int? limit, string q, CancellationToken cancellationToken)
    {
        var query = new GetPostsQuery { Query = q, Limit = limit, Page = page };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("single/{id:int}")]
    public async Task<IActionResult> GetPost(int id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetSinglePostQuery { Id = id };
            var post = await _sender.Send(query, cancellationToken);
            return Ok(post);
        }
        catch (PostException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}