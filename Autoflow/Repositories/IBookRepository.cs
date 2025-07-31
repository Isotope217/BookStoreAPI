using Autoflow.Entities;

namespace Autoflow.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooks(string search, int? rating);
}
