using Autoflow.Dtos;
using Autoflow.Entities;
using Autoflow.Services;
using Microsoft.AspNetCore.Mvc;

namespace Autoflow.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooks(string search, int? rating)
    {
        AuditLog.AddToLog($"GetBooks Search = {search}, Rating = {rating}");

        try
        {
            var books = await _bookService.GetBooks(search, rating);
            AuditLog.AddToLog("Done");
            return Ok(books);
        }
        catch (Exception ex)
        {
            AuditLog.AddToLog("unable to get books: " + ex.Message);
            return StatusCode(500, "Get books request has failed");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateABook([FromBody] BookDto book)
    {
        AuditLog.AddToLog("CreateABook Name: " + book?.Name);

        if (book == null)
            return BadRequest("Book cannot be null.");

        try
        {
            var createdBook = await _bookService.CreateBook(book);
            AuditLog.AddToLog("Done");
            return CreatedAtAction(nameof(GetBooks), new { search = createdBook.Name }, createdBook);
        }
        catch (Exception ex)
        {
            AuditLog.AddToLog("unable to create book: " + ex.Message);
            return StatusCode(500, "unable to create book");
        }
    }
}