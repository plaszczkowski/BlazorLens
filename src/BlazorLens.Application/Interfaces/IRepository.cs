namespace BlazorLens.Application.Interfaces;

public interface IRepository<T> where T : Entity<Guid>
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}