using Autoflow.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AutoflowDbContext _context;

    public BookRepository(AutoflowDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetBooks(string search, int? rating)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                EF.Functions.Like(b.Name, $"%{search}%") ||
                EF.Functions.Like(b.Author, $"%{search}%"));
        }

        if (rating.HasValue)
        {
            query = query.Where(b => b.Rating == rating.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Book> Add(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

}
