using ProductCatalogSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace ProductCatalogSystem.Repositories
{
    public class ProductRepository :Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }

        public override IEnumerable<Product> GetAll()
        {
            return dbSet.Include(x=>x.Inventories).ToList();
        }
        public override Product FirstOrDefault(Expression<Func<Product, bool>> filter)
        {

            return dbSet.Include(x => x.Inventories).FirstOrDefault(filter);
        }
        public override Product GetById(int id)
        {
            return dbSet.Include(x => x.Inventories).FirstOrDefault(p=>p.Id==id);
        }
        public async Task<IEnumerable<Product>> GetProducts(int pageNumber, int pageSize)
        {
            // Include the Inventories for each Product using Eager Loading
            return await dbSet
                .Include(p => p.Inventories) // Eager load related inventories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalProducts()
        {
            return await dbSet.CountAsync();
        }


    }
}
