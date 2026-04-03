using System;
using System.Threading.Tasks;
using GroceryBillingAPI.DTOs;
using GroceryBillingAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroceryBillingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroceryProductsController : ControllerBase
    {
        private readonly IGroceryProductService _productService;
        private readonly ILogger<GroceryProductsController> _logger;

        public GroceryProductsController(IGroceryProductService productService, ILogger<GroceryProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Add a new grocery product
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(GroceryProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroceryProductDto>> AddProduct([FromBody] CreateGroceryProductDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Validation failed", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                var product = await _productService.AddProductAsync(createDto);
                return CreatedAtAction(nameof(AddProduct), new { id = product.Id }, product);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic error while adding product");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding grocery product");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the product", errorId = Guid.NewGuid().ToString() });
            }
        }

        /// <summary>
        /// Get a product by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroceryProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroceryProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Product not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving product");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the product" });
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroceryProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GroceryProductDto>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all products");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving products" });
            }
        }

        /// <summary>
        /// Update a product
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GroceryProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroceryProductDto>> UpdateProduct(int id, [FromBody] CreateGroceryProductDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Validation failed", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                var product = await _productService.UpdateProductAsync(id, updateDto);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Product not found for update");
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic error while updating product");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the product" });
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the product" });
            }
        }
    }
}