using Microsoft.AspNetCore.Mvc;
using ProductCatalogSystem.Core.Interfaces;
using ProductCatalogSystem.Core.Models;
using ProductCatalogSystem.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogSystem.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : SecureAPIController
    {
        private readonly IProductService _service;
        public ProductController(IProductService service, IHttpContextAccessor httpContext) : base(httpContext)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new product")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(CustomBadRequest))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var resp = _service.Create(request, _fullName);
            return Ok(resp);
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "remove a product")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(CustomBadRequest))]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = _service.Delete(_fullName, id);
            return Ok(resp);

        }

        [HttpGet]
        [SwaggerOperation(Summary = "get product list")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<List<Product>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(CustomBadRequest))]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _service.GetAll();
            return Ok(resp);

        }
        [HttpGet]
        [Route("id")]
        [SwaggerOperation(Summary = "get product by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<Product>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(CustomBadRequest))]
        public async Task<IActionResult> Get(int id)
        {
            var resp = _service.GetById(id);
            return Ok(resp);

        }

        [HttpGet]
        [Route("pageNumber/pageSize")]
        [SwaggerOperation(Summary = "get product by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<Product>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(CustomBadRequest))]
        public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var resp = await _service.GetProducts(pageNumber,pageSize);
            return Ok(resp);

        }

    }
}
