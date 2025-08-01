using Autoflow.Dtos;
using Autoflow.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Autoflow.Test.Integration;

public class BookIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BookIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateBook_ReturnsCreated()
    {
        var book = new BookDto(0, "Test Book", "Tester", 4, 9.99M);

        var response = await _client.PostAsJsonAsync("/api/book", book);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var returned = await response.Content.ReadFromJsonAsync<Book>();
        returned!.Name.Should().Be(book.Name);
    }

    [Fact]
    public async Task GetBooks_ShouldReturnInsertedBook()
    {
        // Arrange
        var newBook = new
        {
            Name = "Another Book",
            Author = "Author X",
            Rating = 3,
            Price = 15.00M
        };

        var content = new StringContent(JsonConvert.SerializeObject(newBook), Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/book", content);

        // Act
        var response = await _client.GetAsync("/api/book?search=Another");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<List<BookDto>>(responseString);
        books.Should().ContainSingle(b => b.Name == "Another Book");
    }

    [Fact]
    public async Task CreateBook_ShouldReturnBadRequest_WhenNull()
    {
        // Arrange
        var content = new StringContent("null", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/book", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
