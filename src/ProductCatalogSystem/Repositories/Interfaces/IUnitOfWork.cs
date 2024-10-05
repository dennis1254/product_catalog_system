
using ProductCatalogSystem.Entities;

namespace ProductCatalogSystem.Repositories
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public IRepository<Inventory> InventoryRepository { get; }
        void Save();
    }
}
