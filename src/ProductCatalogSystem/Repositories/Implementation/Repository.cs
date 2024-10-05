using ProductCatalogSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProductCatalogSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
           dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> filter)
        {

            return dbSet.FirstOrDefault(filter);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
        public IEnumerable<T> Where(Expression<Func<T, bool>> filter)
        {
            return dbSet.Where(filter);
        }
        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }

    }
}
