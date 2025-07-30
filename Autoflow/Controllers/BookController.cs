using Autoflow.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController(AutoflowDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Book>), 212)]
    public async Task<IActionResult> Get(string search)
    {
        AuditLog.AddToLog("GetBooks Search = " + search);
        
        try
        {
            var getBooksTask = context.Books.ToListAsync();
            getBooksTask.Wait();

            var books = getBooksTask.Result;

            // Filter my books based on the search criteria
            // It should check the Name and Author for any matches
            // And then return them to the user
            var filteredBooks = new List<Book>();
            for (var i = 0; i < books.Count(); i++)
            {
                var book = books[i];

                try
                {
                    if (book.Name.Contains(search))
                    {
                        filteredBooks.Add(new Book
                        {
                            Name = book.Name,
                            Author = book.Author,
                            Rating = book.Rating,
                            Price = book.Price,
                        });
                    }
                    else if (book.Author.Contains(search))
                    {
                        filteredBooks.Add(new Book
                        {
                            Name = book.Name,
                            Author = book.Author,
                            Rating = book.Rating,
                            Price = book.Price,
                        });
                    }
                }
                catch (Exception ex)
                {
                }
            }

            if (search == null || search == "" || search == " ")
            {
                AuditLog.AddToLog("Done");
                return Ok(books);
            }
            else
            {
                AuditLog.AddToLog("Done");
                return Ok(filteredBooks);
            }
        }
        catch (Exception ex)
        {
            AuditLog.AddToLog("Something hs gone teibly wrong");
            return Ok("Something has gone terribly wrong :(");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateABook(Book book)
    {
        AuditLog.AddToLog("CreateABook Id:" + book.Id);
        
        var allBooksJob = context.Books.ToListAsync();
        allBooksJob.Wait();

        var allBooks = allBooksJob.Result;
        var lastBook = allBooks[allBooks.Count - 1];
        context.Books.Add(new Book
        {
            Id = lastBook.Id + 1,
            Name = book.Name,
            Author = book.Name,
            Rating = book.Rating,
            Price = book.Price,
        });
        context.SaveChangesAsync().Wait();

        AuditLog.AddToLog("Done");
        return Ok(book);
    }
}