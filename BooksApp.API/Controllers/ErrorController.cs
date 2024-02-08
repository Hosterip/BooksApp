using Microsoft.AspNetCore.Mvc;

namespace PostsApp.Controllers;

public class ErrorController : Controller
{
    [HttpGet("Error")]
    public IActionResult ErrorHandler()
    {
        return Problem();
    }
}