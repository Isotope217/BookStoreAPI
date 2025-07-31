using Autoflow.Entities;

namespace Autoflow.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksBySearch(string search, int? rating);
        Task<Book> AddBook(Book book);
    }
}
