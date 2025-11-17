using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Поиск книг по различным критериям. Поддерживает пагинацию и фильтрацию.")]
    [EndpointSummary("Поиск книг")]
    public async Task<ActionResult<IEnumerable<BookResponse>>> SearchBooks([FromQuery] BookFilters filter)
    {
        var result = await _bookService.GetBooks(filter);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Создание новой книги в системе. Требует указания обязательных полей.")]
    [EndpointSummary("Создать новую книгу")]
    public async Task<IActionResult> CreateBook([FromForm] CreateBookRequest request)
    {
        await _bookService.CreateBook(request);

        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Обновление информации о существующей книге. Позволяет изменить описание и стоимость.")]
    [EndpointSummary("Обновить книгу")]
    public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookRequest request)
    {
        await _bookService.UpdateBook(id, request);

        return Ok();
    }
}
