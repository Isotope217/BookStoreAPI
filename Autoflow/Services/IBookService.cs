using Autoflow.Dtos;

namespace Autoflow.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetBooks(string search, int? rating);
    Task<BookDto> CreateBook(BookDto book);
}
