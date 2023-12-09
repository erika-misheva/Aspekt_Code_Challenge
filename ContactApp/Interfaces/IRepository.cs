using ContactApp.Domain.Models;

namespace ContactApp.Interfaces
{
    public interface IRepository<TEntity>
    {
        bool EntityExists(int id);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        bool Saved();
    }
}
