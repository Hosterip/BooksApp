using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Application.Posts.Commands.CreatePost;
using PostsApp.Application.Posts.Commands.DeletePost;
using PostsApp.Application.Posts.Queries.GetPosts;
using PostsApp.Application.Posts.Queries.GetSinglePost;
using PostsApp.Contracts.Requests.Post;
using PostsApp.Domain.Exceptions;
using PostsApp.Shared.Extensions;

namespace PostsApp.Controllers;

[Route("posts")]
public class PostController : Controller
{
    private readonly ISender _sender;

    public PostController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePost(PostRequest request, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to make post");
        if (request.title.IsNullOrEmpty() || request.body.IsNullOrEmpty())
            return BadRequest("Title and body of the post must be filled");
        if (request.title.Length > 255 || request.body.Length > 255)
            return BadRequest("Length of title and body must be less than 255");

        try
        {
            var command = new CreatePostCommand
            {
                Username = HttpContext.Session.GetUserInSession()!, Title = request.title, Body = request.body
            };
            var post = await _sender.Send(command, cancellationToken);

            return StatusCode(201, post);
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
            var command = new DeletePostCommand { Id = id, Username = HttpContext.Session.GetUserInSession()! };
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