using ProductCatalogSystem.Entities;

namespace ProductCatalogSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IProductRepository ProductRepository => new ProductRepository(_db);
        public IRepository<Inventory> InventoryRepository => new Repository<Inventory>(_db);


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
