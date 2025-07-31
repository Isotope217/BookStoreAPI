using Autoflow.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Services
{
    public class BookService : IBookService
    {
        private readonly AutoflowDbContext _context;

        public BookService(AutoflowDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBooksBySearch(string search, int? rating)
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

        public async Task<Book> AddBook(Book book)
        {
            var newBook = new Book
            {
                Name = book.Name,
                Author = book.Author,
                Rating = book.Rating,
                Price = book.Price
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return newBook;
        }
    }
}
