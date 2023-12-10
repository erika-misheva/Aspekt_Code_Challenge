namespace ContactApp.Data.Interfaces;

public interface IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);

    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(int id);

    Task<bool> EntityExistsAsync(int id);
    Task<bool> SavedAsync();
}
