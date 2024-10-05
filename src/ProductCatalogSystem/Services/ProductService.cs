using AutoMapper;
using ProductCatalogSystem.Core.Interfaces;
using ProductCatalogSystem.Core.Models;
using ProductCatalogSystem.Entities;
using ProductCatalogSystem.Repositories;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ProductCatalogSystem.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = Log.ForContext<ProductService>();
            _mapper = mapper;
        }

        public Response<string> Delete(string userId,int id)
        {
           
            try
            {
                var product = _unitOfWork.ProductRepository.FirstOrDefault(x=>x.Id==id);
                _unitOfWork.ProductRepository.Remove(product);
                _unitOfWork.Save();
                return new Response<string>
                {
                    Success = true,
                    Message = "Product deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{method}:: an error occurred while creating Product", nameof(Delete));
                return new Response<string>
                {
                    Success = false,
                    Message = "unsuccessful request"
                };
            }
        }
       
        public Response<string> Create(CreateProductRequest request, string userId)
        {
            try
            {
                var product = _mapper.Map<Product>(request);
                product.CreatedBy = userId;
                _unitOfWork.ProductRepository.Add(product);
                if (product.Inventories!=null&& product.Inventories.Any())
                {
                    var inventories = product.Inventories.Select(x => _mapper.Map<Inventory>(x));
                    _unitOfWork.InventoryRepository.AddRange(inventories);
                }
                _unitOfWork.Save();
                return new Response<string>
                {
                    Success = true,
                    Message = "Product created successfully"
                };
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: an error occurred while creating Product", nameof(Create));
                return new Response<string>
                {
                    Success = false,
                    Message = "unsuccessful request"
                };
            }
        }

        public Response<string> Update(int Id, UpdateProductRequest request, string userId)
        {
            try
            {
                var existingProduct = _unitOfWork.ProductRepository.GetById(Id);
                if (existingProduct != null)
                {
                    existingProduct.ModifiedBy = userId;
                    existingProduct.ModifiedAt = DateTime.Now;
                    existingProduct.Name = request.Name;
                    existingProduct.Description = request.Description;
                    _unitOfWork.ProductRepository.Update(existingProduct);
                    _unitOfWork.Save();
                    return new Response<string>
                    {
                        Success = true,
                        Message = "Product updated successfully"
                    };
                }
                else
                {
                    return new Response<string>
                    {
                        Success = false,
                        Message = $"No existing product with Id - {Id} found"
                    };
                }
               
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: an error occurred while creating Product", nameof(Create));
                return new Response<string>
                {
                    Success = false,
                    Message = "unsuccessful request"
                };
            }
        }

        public Response<Product> GetById(int id)
        {
            try
            {
                var product = _unitOfWork.ProductRepository.GetById(id);
                return new Response<Product>
                {
                    Success = product!= null,
                    Message = product != null?"Successful":"No record found",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{method}:: an error occurred while retrieving Product", nameof(GetById));
                return new Response<Product>
                {
                    Success = false,
                    Message = "unsuccessful request"
                };
            }
        }

        public async Task<Response<IEnumerable<Product>>> GetAll()
        {
            try
            {
                var products = _unitOfWork.ProductRepository.GetAll();
                return new Response<IEnumerable<Product>>
                {
                    Success = (products != null && products.Any()),
                    Message = (products != null && products.Any()) ? "Successful" : "No record found",
                    Data = products
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{method}:: an error occurred while retrieving Product", nameof(GetById));
                return new Response<IEnumerable<Product>>
                {
                    Success = false,
                    Message = "unsuccessful request"
                };
            }
        }
        public async Task<PaginatedResponse<Product>> GetProducts(int pageNumber, int pageSize)
        {
            try
            {
               
                var products = await _unitOfWork.ProductRepository.GetProducts(pageNumber, pageSize);
                var totalProducts = await _unitOfWork.ProductRepository.GetTotalProducts();

                // Create a paginated response object
                var response = new PaginatedResponse<Product>(products.ToList(), pageNumber, pageSize, totalProducts);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{method}:: an error occurred while retrieving Product", nameof(GetById));
                return new PaginatedResponse<Product>(new List<Product>(),pageNumber,pageSize,0)
                {
                    Success = false,
                    Message = "unsuccessful request"
                };
            }
        }
    }
}
