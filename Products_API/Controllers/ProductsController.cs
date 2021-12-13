using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Products_API.Models;
using Products_Repo.Models;
using Products_Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger _logger;


        public ProductsController(IProductRepo productRepo,
                                 ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _logger = logger;


        }

        ///// <summary>
        /////     Search for products that match the query.
        ///// </summary>
        ///// <remarks>
        /////     Sample request:
        /////     GET api/Catalog?page=1
        ///// </remarks>
        ///// <returns>A list of the matching products</returns>
        ///// <response code="200">Returns a list of the matching products.</response>
        ///// <response code="400">If the query is not valid.</response>
        //[HttpGet]
        //[Produces(MediaType.ApplicationJson)]
        //[ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetCatalog(
        //    [FromQuery] GetCatalogDto getCatalog)
        //{
        //    _logger.LogThisMethod();
        //    var products = await _catalogService.GetCatalogPageAsync(getCatalog);
        //    return Ok(products);
        //}

        /// <summary>
        ///     Search for products that match the query.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET api/Catalog/GetProduct?name=lamp&amp;minPrice=10
        /// </remarks>
        /// <returns>A list of the matching products.</returns>
        /// <response code="200">Returns a list of the matching products.</response>
        /// <response code="400">If the query is not valid.</response>
        [HttpGet]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepo.GetProducts();
            return Ok(products);
        }

        /// <summary>
        ///     Search for a product by Id
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET api/Catalog/GetProduct?id=8916c69a-8041-4768-8e0d-a391361ff732
        /// </remarks>
        /// <param name="id">Id of the Product to search</param>
        /// <returns>A Product with matching Id</returns>
        /// <response code="200">Returns a Product with matching Id</response>
        /// <response code="400">If the Id does not exist</response>
        [HttpGet]
        [Route("[action]", Name = nameof(GetProduct))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct([FromQuery] Guid id)
        {
            var product = await _productRepo.GetProduct(id);
            if (product.Sku == null)
                return NotFound(id);
            return Ok(product);
        }

        /// <summary>
        ///     Add a Product to the catalog
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST api/Catalog/AddProduct
        ///     {"name":"new product name", "price":"52.99",
        ///     "photo":"https://www.bhphotovideo.com/images/images2500x2500/dell_u2417h_24_16_9_ips_1222870.jpg"}
        /// </remarks>
        /// <param name="newProduct">Product to add</param>
        /// <returns>A newly-created Product</returns>
        /// <response code="201">Returns the newly-created Product</response>
        /// <response code="400">If the Product is not valid</response>
        [HttpPut]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct([FromBody] Product newProduct)
        {
            var idProductAdded = await _productRepo.AddProductAsync(newProduct);
            var uri = Url.Link(nameof(GetProduct), new { id = idProductAdded });
            return Created(uri, newProduct);
        }

        /// <summary>
        ///     Edit a Product in the catalog
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     PUT api/Catalog/EditProduct?id=8916c69a-8041-4768-8e0d-a391361ff732
        ///     {"name":"test modified", "price":"52.99", "photo":"test2new.png"}
        /// </remarks>
        /// <param name="id">Id of the Product to edit</param>
        /// <param name="editProduct">Product edited</param>
        /// <returns>The updated Product</returns>
        /// <response code="200">Returns the updated Product</response>
        /// <response code="400">If the Id does not exist in the Catalog</response>
        [HttpPut]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditProduct([FromQuery] Guid id,
                                                     [FromBody] Product editProduct)
        {
            var productEdited = await _productRepo.EditProductAsync(id, editProduct);
            if (productEdited.Sku == null)
                return NotFound(id);
            return Ok(productEdited);
        }

        /// <summary>
        ///     Delete a Product in the catalog
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     DELETE api/Catalog/DeleteProduct?id=8916c69a-8041-4768-8e0d-a391361ff732
        /// </remarks>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>The deleted Product</returns>
        /// <response code="200">Returns the deleted Product</response>
        /// <response code="400">If the Id does not exist in the Catalog</response>
        [HttpDelete]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProduct([FromQuery] Guid id)
        {
            var productDeleted = await _productRepo.DeleteProductAsync(id);
            if (productDeleted.Sku == null)
                return BadRequest();
            return Ok(productDeleted);
        }

    }
}

