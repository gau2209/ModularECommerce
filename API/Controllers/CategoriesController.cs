using Application.Categories.DTOs;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories ()
        {
            var categories = await _categoryService.GetPublicCategoriesAsync( );

            return Ok(categories);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategoriesForAdmin ()
        {
            var categories = await _categoryService.GetAdminCategoriesAsync( );

            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryByID (Guid id)
        {
            var category = await _categoryService.GetByIDAsync(id);

            if ( category == null )
                return NotFound( );

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory (CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetCategoryByID),
                new { id = category.ID },
                category);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory (Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateAsync(id, request);

            if ( category == null )
                return NotFound( );

            return Ok(category);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory (Guid id)
        {
            var deleted = await _categoryService.DeleteAsync(id);

            if ( !deleted )
                return NotFound( );

            return NoContent( );
        }

        [HttpPatch("{id:guid}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateCategory (Guid id)
        {
            var updated = await _categoryService.ActivateAsync(id);

            if ( !updated )
                return NotFound( );

            return NoContent( );
        }

        [HttpPatch("{id:guid}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateCategory (Guid id)
        {
            var updated = await _categoryService.DeactivateAsync(id);

            if ( !updated )
                return NotFound( );

            return NoContent( );
        }
    }
}
