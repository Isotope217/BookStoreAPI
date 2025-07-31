using Autoflow.Entities;
using Autoflow.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Test.Unit;

public class BookRepositoryTest
{
    private static AutoflowDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AutoflowDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AutoflowDbContext(options);
    }

    [Fact]
    public async Task Create_AddsBookToDatabase()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext("Create_AddsBook");
        var repository = new BookRepository(dbContext);

        var book = new Book
        {
            Name = "Test Book",
            Author = "Author A",
            Rating = 4,
            Price = 10.99m
        };

        // Act
        var result = await repository.Add(book);

        // Assert
        var booksInDb = await dbContext.Books.ToListAsync();
        Assert.Single(booksInDb);
        Assert.Equal("Test Book", booksInDb[0].Name);
        Assert.Equal(result.Id, booksInDb[0].Id);
    }

    [Fact]
    public async Task GetBySearch_ReturnsFilteredBooks()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext("GetBySearch_Filters");
        dbContext.Books.AddRange(
            new Book { Name = "1984", Author = "Orwell", Rating = 5, Price = 12 },
            new Book { Name = "Brave New World", Author = "Huxley", Rating = 4, Price = 15 },
            new Book { Name = "The Hobbit", Author = "Tolkien", Rating = 5, Price = 18 }
        );
        await dbContext.SaveChangesAsync();

        var repository = new BookRepository(dbContext);

        // Act
        var results = await repository.GetBooks("hobbit", null);

        // Assert
        Assert.Single(results);
        Assert.Contains(results, b => b.Name == "The Hobbit");
    }

    [Fact]
    public async Task GetBySearch_WithRating_ReturnsCorrectBooks()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext("GetBySearch_WithRating");
        dbContext.Books.AddRange(
            new Book { Name = "Book A", Author = "Author A", Rating = 3, Price = 10 },
            new Book { Name = "Book B", Author = "Author B", Rating = 4, Price = 12 },
            new Book { Name = "Book C", Author = "Author C", Rating = 3, Price = 14 }
        );
        await dbContext.SaveChangesAsync();

        var repository = new BookRepository(dbContext);

        // Act
        var results = await repository.GetBooks("", 3);

        // Assert
        Assert.Equal(2, results.Count());
        Assert.All(results, b => Assert.Equal(3, b.Rating));
    }
}
