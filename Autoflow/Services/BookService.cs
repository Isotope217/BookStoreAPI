using Autoflow.Dtos;
using Autoflow.Entities;
using Autoflow.Repositories;

namespace Autoflow.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookDto>> GetBooks(string search, int? rating)
    {
        var books = await _bookRepository.GetBooks(search, rating);

        var bookDtos = books.Select(book => new BookDto
        (
            book.Id,
            book.Name,
            book.Author,
            book.Rating,
            book.Price
        ));

        return bookDtos;
    }

    public async Task<BookDto> CreateBook(BookDto book)
    {
        var Newbook = new Book
        {
            Name = book.Name,
            Author = book.Author,
            Rating = book.Rating,
            Price = book.Price
        };

        var createdBook = await _bookRepository.Add(Newbook);

        return new BookDto(
            createdBook.Id,
            createdBook.Name,
            createdBook.Author,
            createdBook.Rating,
            createdBook.Price
        );
    }
}
