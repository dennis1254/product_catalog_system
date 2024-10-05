using ProductCatalogSystem.Infrastructure.Models;

namespace ProductCatalogSystem.Infrastructure.Interfaces
{
    public interface ICBAPosting
    {
        Task<(SinglePostResponse Data, bool Success)> SinglePost(SinglePostRequest request);
        Task<(BatchPostResponse Data, bool Success)> BatchPost(BatchPostRequest request);
    }
}
