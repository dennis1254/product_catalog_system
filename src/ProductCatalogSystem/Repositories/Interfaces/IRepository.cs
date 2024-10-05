using System.Linq.Expressions;

namespace ProductCatalogSystem.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        T FirstOrDefault(Expression<Func<T, bool>> filter);
        void UpdateRange(IEnumerable<T> entities);
        void AddRange(IEnumerable<T> entities);
    }
}
