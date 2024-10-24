using DataService.Data;
using DataService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataService.Repository;

public class GenericRepostory<T> : IGenericRepository<T> where T : class
{
    public readonly ILogger _logger;
    protected AppDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepostory(
        AppDbContext context,
        ILogger logger)
    {
        _context = context;       
        _logger = logger;

        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> Create(T entity)
    {
        _context.ChangeTracker.Clear();
        var entityEntry = await _dbSet.AddAsync(entity);
        return entityEntry?.Entity;
    }
}