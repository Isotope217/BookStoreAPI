namespace Autoflow.Test.Unit;

using Autoflow.Dtos;
using Autoflow.Entities;
using Autoflow.Repositories;
using Autoflow.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _bookService = new BookService(_bookRepositoryMock.Object);
    }

    //public static readonly TheoryData<Book[], string, int?, BookDto> BookSearchCases = new()
    //{
    //    {
    //        new Book[]
    //        {
    //            new() { Id = 100, Name = "To Kill a Mockingbird", Author = "Harper Lee", Rating = 1, Price = 12.99m },
    //            new() { Id = 2, Name = "1984", Author = "George Orwell", Rating = 2, Price = 9.99m }
    //        },
    //        "Kill", null,
    //        new BookDto(100, "To Kill a Mockingbird", "Harper Lee", 1, 12.99m)
    //    },
    //    {
    //        new Book[]
    //        {
    //            new() { Id = 100, Name = "To Kill a Mockingbird", Author = "Harper Lee", Rating = 1, Price = 12.99m },
    //            new() { Id = 101, Name = "1984", Author = "George Orwell", Rating = 2, Price = 9.99m }
    //        },
    //        null, 2,
    //        new BookDto(101, "1984", "George Orwel", 2, 9.99m)
    //    }
    //};


    //[Theory]
    //[MemberData(nameof(BookSearchCases))]
    //public async Task GetBooks_ReturnsFilteredBooks(
    //    Book[] repositoryData,
    //    string search,
    //    int? rating,
    //    BookDto expected)
    //{
    //    // Arrange
    //    _bookRepositoryMock
    //        .Setup(r => r.GetBooks(search, rating))
    //        .ReturnsAsync(repositoryData.Where(b => b.Rating == 2).ToList());

    //    // Act
    //    var result = await _bookService.GetBooks(search, rating);

    //    // Assert
    //    var book = Assert.Single(result);
    //    Assert.Equal(expected, book);
    //}

    [Fact]
    public async Task GetBooks_WithNullParams_ReturnsAllBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 100, Name = "To Kill a Mockingbird", Author = "Harper Lee", Rating = 1, Price = 12.99m },
            new Book { Id = 101, Name = "1984", Author = "George Orwell", Rating = 2, Price = 9.99m }
        };

        _bookRepositoryMock
            .Setup(repo => repo.GetBooks(null, null))
            .ReturnsAsync(books);

        // Act
        var result = await _bookService.GetBooks(null, null);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetBooks_WithRatingOnly_ReturnsMatchingBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 100, Name = "To Kill a Mockingbird", Author = "Harper Lee", Rating = 1, Price = 12.99m },
            new Book { Id = 101, Name = "1984", Author = "George Orwell", Rating = 2, Price = 9.99m }
        };

        _bookRepositoryMock
            .Setup(repo => repo.GetBooks(null, 2))
            .ReturnsAsync(books.Where(b => b.Rating == 2).ToList());

        // Act
        var result = await _bookService.GetBooks(null, 2);

        // Assert
        var book = Assert.Single(result);
        Assert.Equal("1984", book.Name);
    }

    [Fact]
    public async Task GetBooks_WithSearchOnly_ReturnsMatchingBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 100, Name = "To Kill a Mockingbird", Author = "Harper Lee", Rating = 1, Price = 12.99m },
            new Book { Id = 101, Name = "1984", Author = "George Orwell", Rating = 2, Price = 9.99m }
        };

        _bookRepositoryMock
            .Setup(repo => repo.GetBooks("1984", null))
            .ReturnsAsync(books.Where(b => b.Name.Contains("1984")).ToList());

        // Act
        var result = await _bookService.GetBooks("1984", null);

        // Assert
        var book = Assert.Single(result);
        Assert.Equal("1984", book.Name);
    }

    [Fact]
    public async Task GetBooks_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        _bookRepositoryMock
            .Setup(repo => repo.GetBooks("Nonexistent", 1))
            .ReturnsAsync(new List<Book>());

        // Act
        var result = await _bookService.GetBooks("Nonexistent", 1);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddBook_ShouldAddBookAndReturnBookDto()
    {
        // Arrange
        var bookDto = new BookDto(0, "New Book", "A Author", 5, 30m);
        var bookEntity = new Book { Id = 1, Name = "New Book", Author = "A Author", Rating = 5, Price = 30m };

        _bookRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Book>()))
            .ReturnsAsync(bookEntity);

        // Act
        var result = await _bookService.CreateBook(bookDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Book", result.Name);
        Assert.Equal("A Author", result.Author);
        Assert.Equal(5, result.Rating);
        Assert.Equal(30m, result.Price);
    }
}