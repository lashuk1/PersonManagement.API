using Microsoft.EntityFrameworkCore;
using PersonManagement.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace PersonManagement.Infrastructure.Repositories.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public T Update(T entity)
    {
        var result = _context.Set<T>().Update(entity);
        return result.Entity;
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(ICollection<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _context.Set<T>().AsNoTracking();

        foreach (var property in includeProperties)
            query = query.Include(property);

        return await query.SingleOrDefaultAsync(predicate);
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
}
