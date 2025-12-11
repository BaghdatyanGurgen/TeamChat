using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories.Base;

public class BasicRepository<TEntity, TKey>(AppDbContext context)
    : IBasicRepository<TEntity, TKey> where TEntity : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<bool> ExistsAsync(TKey id)
        => await _dbSet.FindAsync(id) is not null;

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}