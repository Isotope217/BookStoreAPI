using Autoflow.Controllers;
using Autoflow.Dtos;
using Autoflow.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Autoflow.Test.Unit;

public class BookControllerTest
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly BookController _controller;

    public BookControllerTest()
    {
        _mockBookService = new Mock<IBookService>();
        _controller = new BookController(_mockBookService.Object);
    }

    [Fact]
    public async Task GetBooks_ReturnsOk_WithBooks()
    {
        // Arrange
        var books = new List<BookDto>
        {
            new BookDto(1, "Test", "Author", 4, 10.0m)
        };

        _mockBookService
            .Setup(s => s.GetBooks("test", 4))
            .ReturnsAsync(books);

        // Act
        var result = await _controller.GetBooks("test", 4);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<BookDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetBooks_Returns500_OnException()
    {
        // Arrange
        _mockBookService
            .Setup(s => s.GetBooks(It.IsAny<string>(), It.IsAny<int?>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await _controller.GetBooks("oops", null);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Get books request has failed", objectResult.Value);
    }

    [Fact]
    public async Task CreateABook_ReturnsCreatedAtAction()
    {
        // Arrange
        var bookDto = new BookDto(0, "New Book", "Author", 5, 12.99m);

        _mockBookService
            .Setup(s => s.CreateBook(bookDto))
            .ReturnsAsync(bookDto);

        // Act
        var result = await _controller.CreateABook(bookDto);

        // Assert
        var createdAt = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<BookDto>(createdAt.Value);
        Assert.Equal("New Book", returnValue.Name);
    }

    [Fact]
    public async Task CreateABook_ReturnsBadRequest_WhenNull()
    {
        // Act
        var result = await _controller.CreateABook(null);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Book cannot be null.", badRequest.Value);
    }

    [Fact]
    public async Task CreateABook_Returns500_OnException()
    {
        // Arrange
        var bookDto = new BookDto(0, "New Book", "Author", 5, 12.99m);

        _mockBookService
            .Setup(s => s.CreateBook(null))
            .ThrowsAsync(new Exception("oops"));

        // Act
        var result = await _controller.CreateABook(bookDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("unable to create book", objectResult.Value);
    }
}
