namespace Autoflow.Repositories;

public interface IRepository<T>
{
    Task<T> Add(T entity);
}
