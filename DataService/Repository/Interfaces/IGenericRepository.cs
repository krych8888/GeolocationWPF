namespace DataService.Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> Create(T entity);
}
