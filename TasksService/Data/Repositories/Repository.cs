using Microsoft.EntityFrameworkCore;
using TasksService.Data;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly GranjaDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(GranjaDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) throw new KeyNotFoundException();
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}