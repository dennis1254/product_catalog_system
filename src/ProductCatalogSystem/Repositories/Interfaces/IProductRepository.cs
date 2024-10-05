using ProductCatalogSystem.Entities;
using System.Linq.Expressions;

namespace ProductCatalogSystem.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProducts(int pageNumber, int pageSize);
        Task<int> GetTotalProducts();
    }
}
