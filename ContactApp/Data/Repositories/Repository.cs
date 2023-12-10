using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data.Repositories;

using ContactApp.Data.Interfaces;
using Data;
using Domain.Models;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
{
    protected readonly IContactDbContext _context;

    public Repository(IContactDbContext contactDbContext)
    {
        _context = contactDbContext ?? throw new ArgumentNullException(nameof(contactDbContext));
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        TEntity? entityToDelete = await _context.Set<TEntity>().FindAsync(id);
        if (entityToDelete != null)
        {
            _context.Set<TEntity>().Remove(entityToDelete);
        }
    }

    public async Task<bool> EntityExistsAsync(int id)
    {
        return await _context.Set<TEntity>().AnyAsync(entity => entity.Id == id);
    }
    public async Task<bool> SavedAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
