using BooksApp.API.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BooksApp.API.Controllers;

public class ErrorsController : ApiController
{
    [HttpGet(ApiRoutes.Error.ErrorHandler)]
    public IActionResult ErrorHandler()
    {
        return Problem();
    }
}