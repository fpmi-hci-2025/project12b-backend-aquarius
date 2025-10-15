using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetAllBooks()
    {
        return Ok();
    }
}
