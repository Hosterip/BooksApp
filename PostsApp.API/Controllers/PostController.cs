using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Application.Services.Posts;
using PostsApp.Contracts.Requests.Post;
using PostsApp.Domain.Exceptions;
using PostsApp.Shared.Extensions;

namespace PostsApp.Controllers;

[Route("posts")]
public class PostController : Controller
{
    private readonly IPostsService _postsService;

    public PostController(IPostsService postsService)
    {
        _postsService = postsService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePost(PostRequest postRequest)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to make post");
        if (postRequest.title.IsNullOrEmpty() || postRequest.body.IsNullOrEmpty())
            return BadRequest("Title and body of the post must be filled");
        if (postRequest.title.Length > 255 || postRequest.body.Length > 255)
            return BadRequest("Length of title and body must be less than 255");

        await _postsService.CreatePost(postRequest.title, postRequest.body, HttpContext.Session.GetUserInSession()!);

        return StatusCode(201, "Post has created");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized to delete post");

        try
        {
            await _postsService.DeletePost(id, HttpContext.Session.GetUserInSession()!);
            return Ok("Post was deleted");
        }
        catch (PostException)
        {
            return BadRequest("Post not found or post not yours");
        }
    }

    [HttpGet("many/{page:int}")]
    public IActionResult GetPosts(int page)
    {
        try
        {
            string? query = Request.Query["q"];
            string? strLimit = Request.Query["limit"];
            int intLimit = !strLimit.IsNullOrEmpty() ? Convert.ToInt32(strLimit) : 10;
            return Ok(_postsService.GetPosts(page, intLimit, query ?? ""));
        }
        catch (FormatException)
        {
            return BadRequest("Limit must be an integer");
        }
    }
    [HttpGet("single/{id:int}")]
    public IActionResult GetPost(int id)
    {
        try
        {
            return Ok(_postsService.GetSinglePost(id));
        }
        catch (PostException)
        {
            return BadRequest("Post not found");
        }
    }
}