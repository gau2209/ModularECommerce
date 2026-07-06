using Application.Common.Interfaces;
using Application.Products.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;

        public ProductsController (IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProducts ([FromQuery] ProductSearchRequest request)
        {
            var result = await _productService.SearchAsync(request);

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts ()
        {
            var products = await _productService.GetPublicProductsAsync( );

            return Ok(products);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductsForAdmin ()
        {
            var products = await _productService.GetAdminProductsAsync( );

            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByID (Guid id)
        {
            var product = await _productService.GetByIDAsync(id);

            if ( product == null )
                return NotFound( );

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct (CreateProductRequest request)
        {
            var product = await _productService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetProductByID),
                new { id = product.ID },
                product);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct (Guid id, UpdateProductRequest request)
        {
            var product = await _productService.UpdateAsync(id, request);

            if ( product == null )
                return NotFound( );

            return Ok(product);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct (Guid id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if ( !deleted )
                return NotFound( );

            return Ok("Delete Successfully");
        }

        [HttpPatch("{id:guid}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateProduct (Guid id)
        {
            var updated = await _productService.ActivateAsync(id);

            if ( !updated )
                return NotFound( );

            return Ok("Activate Successfully");
        }

        [HttpPatch("{id:guid}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateProduct (Guid id)
        {
            var updated = await _productService.DeactivateAsync(id);

            if ( !updated )
                return NotFound( );

            return Ok("Deactivate Successfully");
        }
    }
}