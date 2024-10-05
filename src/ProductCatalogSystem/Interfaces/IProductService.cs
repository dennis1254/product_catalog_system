using ProductCatalogSystem.Core.Models;
using ProductCatalogSystem.Entities;

namespace ProductCatalogSystem.Core.Interfaces
{
    public interface IProductService
    {
        Response<string> Create(CreateProductRequest request, string userId);
        Response<string> Update(UpdateProductRequest request, string userId);
        Response<string> Delete(string userId,int id);
        Response<Product> GetById(int id);
        Task<Response<IEnumerable<Product>>> GetAll();
        Task<PaginatedResponse<Product>> GetProducts(int pageNumber, int pageSize);


    }
}
