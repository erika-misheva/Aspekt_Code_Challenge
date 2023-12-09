using ContactApp.Domain.Models;

namespace ContactApp.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<bool> EntityExistsAsync(int id);
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<bool> SavedAsync();
    }
}
