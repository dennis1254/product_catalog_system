using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalogSystem.Core.Models;
using ProductCatalogSystem.Core.Services;
using ProductCatalogSystem.Entities;
using ProductCatalogSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductCatalogSystem.Test
{
    public class ProductServiceTest
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("database")
                .Options;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<Automapper>());
            mapper = config.CreateMapper();
            var context = new AppDbContext(options);
            _unitOfWork = new UnitOfWork(context);
            _productService = new ProductService(_unitOfWork,mapper);
        }

        // Test for paginated product retrieval
        [Fact]
        public async Task GetProducts_ShouldReturnPaginatedList()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Name = "Product 1", Description = "first test product", CreatedBy = "XUnit",
                Inventories = new List<Inventory>
                {
                    new Inventory { Location = "Ajah branch", Quantity = 5},
                    new Inventory { Location = "Oshodi branch", Quantity = 30},
                }
            },
            new Product { Name = "Product 2", Description = "second test product", CreatedBy = "XUnit",
                Inventories = new List<Inventory>
                {
                    new Inventory { Location = "Ajah branch", Quantity = 20},
                    new Inventory { Location = "Oshodi branch", Quantity = 10},
                }
            }
        };

            _unitOfWork.ProductRepository.AddRange(products);
            _unitOfWork.Save();

            // Act
            var result = await _productService.GetProducts(1, 3);
            var totalProduct = await _unitOfWork.ProductRepository.GetTotalProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(3, result.PageSize);
            Assert.Equal(totalProduct, result.TotalItems);
            Assert.Equal((int)System.Math.Ceiling(totalProduct / (double)3), result.TotalPages); // Total items: 3, pageSize: 3 -> 1 pages
        }

        // Test for retrieving a product by ID
        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product {Id = 100, Name = "Product 100", Description = "100 test product", CreatedBy = "XUnit",
                Inventories = new List<Inventory>
                {
                    new Inventory { Location = "Ajah branch", Quantity = 5},
                    new Inventory { Location = "Oshodi branch", Quantity = 30},
                }
            },
            new Product {Id = 200, Name = "Product 200", Description = "200 test product", CreatedBy = "XUnit",
                Inventories = new List<Inventory>
                {
                    new Inventory { Location = "Ajah branch", Quantity = 20},
                    new Inventory { Location = "Oshodi branch", Quantity = 10},
                }
            }
        };

            _unitOfWork.ProductRepository.AddRange(products);
            _unitOfWork.Save();

            // Act
            var result = _productService.GetById(200);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.Data.Id);
            Assert.Equal("Product 200", result.Data.Name);
        }

        // Test for adding a new product
        [Fact]
        public async Task AddProduct_ShouldCallRepository_WhenProductIsValid()
        {
            // Arrange
            var product = new CreateProductRequest
            {
                Name = "Product 10",
                Description = "Tenth test product",
                Inventories = new List<CreateInventoryRequest>
                {
                    new CreateInventoryRequest { Location = "Ajah branch", Quantity = 5 },
                    new CreateInventoryRequest { Location = "Oshodi branch", Quantity = 30 },
                }
            };
            // Act
            var result = _productService.Create(product,"XUnit");

            // Assert
            Assert.True(result.Success);
        }

       

        // Test for deleting a product
        [Fact]
        public async Task DeleteProduct_ShouldCallRepository_WhenProductExists()
        {
            // Arrange
            var getProduct = _unitOfWork.ProductRepository.FirstOrDefault(x => x.Id > 0);

            // Act
             var result = _productService.Delete("XUnit", getProduct.Id);

            // Assert
            Assert.True(result.Success);
        }

        // Test for deleting a product when it doesn't exist
        [Fact]
        public async Task DeleteProduct_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange

            // Act & Assert
            var result = _productService.Delete("XUnit",100);
            Assert.False(result.Success);
        }
    }
}