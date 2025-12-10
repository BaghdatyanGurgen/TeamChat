namespace TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

public interface IBasicRepository<T, TKey> where T : class
{
    Task<T?> GetByIdAsync(TKey id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task<bool> ExistsAsync(TKey id);
}