using Microsoft.EntityFrameworkCore;
using RouteBookingSystem.Data;
using System.Linq.Expressions;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly XKKBUSContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(XKKBUSContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>() ?? throw new InvalidOperationException($"DbSet<{typeof(T).Name}> не инициализирован.");
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync() ?? new List<T>();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return await _dbSet.Where(predicate).ToListAsync() ?? new List<T>();
    }

    public async Task AddAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _dbSet.Remove(entity);
    }
}