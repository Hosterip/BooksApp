using Microsoft.AspNetCore.Mvc;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public class ErrorsController : ApiController
{
    [HttpGet(ApiRoutes.Error.ErrorHandler)]
    public IActionResult ErrorHandler()
    {
        return Problem();
    }
}