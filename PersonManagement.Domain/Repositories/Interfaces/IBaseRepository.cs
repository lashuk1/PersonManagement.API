namespace PersonManagement.Infrastructure.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    T Update(T entity);
    void Remove(T entity);
    void RemoveRange(ICollection<T> entities);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
}