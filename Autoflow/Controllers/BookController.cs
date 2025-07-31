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
    public async Task<IActionResult> GetBook(string search, int? rating)
    {
        AuditLog.AddToLog($"GetBooks Search = {search}, Rating = {rating}");

        try
        {
            var books = await _bookService.GetBooksBySearch(search, rating);
            AuditLog.AddToLog("Done");
            return Ok(books);
        }
        catch (Exception ex)
        {
            AuditLog.AddToLog("Something has gone terribly wrong: " + ex.Message);
            return StatusCode(500, "Get books request has failed");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateABook([FromBody] Book book)
    {
        AuditLog.AddToLog("CreateABook Name: " + book?.Name);

        if (book == null)
            return BadRequest("Book cannot be null.");

        try
        {
            var createdBook = await _bookService.AddBook(book);
            AuditLog.AddToLog("Done");
            return CreatedAtAction(nameof(GetBook), new { search = createdBook.Name }, createdBook);
        }
        catch (Exception ex)
        {
            AuditLog.AddToLog("Something has gone terribly wrong: " + ex.Message);
            return StatusCode(500, "Something has gone terribly wrong :(");
        }
    }

    //[HttpGet]
    //[ProducesResponseType(typeof(List<Book>), 212)]
    //public async Task<IActionResult> Get(string search)
    //{
    //    AuditLog.AddToLog("GetBooks Search = " + search);

    //    try
    //    {
    //        var getBooksTask = context.Books.ToListAsync();
    //        getBooksTask.Wait();

    //        var books = getBooksTask.Result;

    //        // Filter my books based on the search criteria
    //        // It should check the Name and Author for any matches
    //        // And then return them to the user
    //        var filteredBooks = new List<Book>();
    //        for (var i = 0; i < books.Count(); i++)
    //        {
    //            var book = books[i];

    //            try
    //            {
    //                if (book.Name.Contains(search))
    //                {
    //                    filteredBooks.Add(new Book
    //                    {
    //                        Name = book.Name,
    //                        Author = book.Author,
    //                        Rating = book.Rating,
    //                        Price = book.Price,
    //                    });
    //                }
    //                else if (book.Author.Contains(search))
    //                {
    //                    filteredBooks.Add(new Book
    //                    {
    //                        Name = book.Name,
    //                        Author = book.Author,
    //                        Rating = book.Rating,
    //                        Price = book.Price,
    //                    });
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //            }
    //        }

    //        if (search == null || search == "" || search == " ")
    //        {
    //            AuditLog.AddToLog("Done");
    //            return Ok(books);
    //        }
    //        else
    //        {
    //            AuditLog.AddToLog("Done");
    //            return Ok(filteredBooks);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        AuditLog.AddToLog("Something hs gone teibly wrong");
    //        return Ok("Something has gone terribly wrong :(");
    //    }
    //}

    //[HttpPost]
    //public async Task<IActionResult> CreateABook(Book book)
    //{
    //    AuditLog.AddToLog("CreateABook Id:" + book.Id);

    //    var allBooksJob = context.Books.ToListAsync();
    //    allBooksJob.Wait();

    //    var allBooks = allBooksJob.Result;
    //    var lastBook = allBooks[allBooks.Count - 1];
    //    context.Books.Add(new Book
    //    {
    //        Id = lastBook.Id + 1,
    //        Name = book.Name,
    //        Author = book.Name,
    //        Rating = book.Rating,
    //        Price = book.Price,
    //    });
    //    context.SaveChangesAsync().Wait();

    //    AuditLog.AddToLog("Done");
    //    return Ok(book);
    //}
}