using Application.Categories.DTOs;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService (AppDbContext appContext)
        {
            _context = appContext;
        }

        public async Task<List<CategoryDto>> GetPublicCategoriesAsync ()
        {
            return await _context.Categories.AsNoTracking()
                .Where(x=>x.IsActive && !x.IsDeleted)
                .OrderBy(x=>x.Name)
                .Select(x=> ToDto(x))
                .ToListAsync();
        }

        public async Task<List<CategoryDto>> GetAdminCategoriesAsync ()
        {
            return await _context.Categories
            .AsNoTracking( )
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => ToDto(x))
            .ToListAsync( );
        }

        public async Task<CategoryDto?> GetByIDAsync (Guid ID)
        {
            return await _context.Categories
            .AsNoTracking( )
            .Where(x => x.ID == ID && !x.IsDeleted)
            .Select(x => ToDto(x))
            .FirstOrDefaultAsync( );
        }

        public async Task<CategoryDto> CreateAsync (CreateCategoryRequest request)
        {
            var name = request.Name.Trim( );
            var slug = NormalizeSlug(request.Slug);

            if ( !string.IsNullOrWhiteSpace(slug) )
            {
                var slugExists = await _context.Categories
                    .AnyAsync(x => !x.IsDeleted && x.Slug == slug);

                if ( slugExists )
                    throw new InvalidOperationException("Category slug already exists.");
            }

            if ( request.ParentID.HasValue )
            {
                var parentExists = await _context.Categories
                    .AnyAsync(x => x.ID == request.ParentID.Value && !x.IsDeleted);

                if ( !parentExists )
                    throw new InvalidOperationException("Parent category does not exist.");
            }

            var category = new Category(name, slug, request.Description, request.ParentID);
          

            _context.Categories.Add(category);
            await _context.SaveChangesAsync( );

            return ToDto(category);
        }

        public async Task<CategoryDto?> UpdateAsync (Guid ID, UpdateCategoryRequest request)
        {
            var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.ID == ID && !x.IsDeleted);

            if ( category == null )
                return null;

            var name = request.Name.Trim( );
            var slug = NormalizeSlug(request.Slug);

            if ( !string.IsNullOrWhiteSpace(slug) )
            {
                var slugExists = await _context.Categories
                    .AnyAsync(x => !x.IsDeleted && x.Slug == slug && x.ID != ID);

                if ( slugExists )
                    throw new InvalidOperationException("Category slug already exists.");
            }

            if ( request.ParentID.HasValue )
            {
                if ( request.ParentID.Value == ID )
                    throw new InvalidOperationException("Category cannot be its own parent.");

                var parentExists = await _context.Categories
                    .AnyAsync(x => x.ID == request.ParentID.Value && !x.IsDeleted);

                if ( !parentExists )
                    throw new InvalidOperationException("Parent category does not exist.");
            }

            category.Update(name, slug, string.Empty, request.ParentID, request.IsActive);

            await _context.SaveChangesAsync( );

            return ToDto(category);
        }

        public async Task<bool> ActivateAsync (Guid ID)
        {
            var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.ID == ID && !x.IsDeleted);

            if ( category == null )
                return false;

            category.Activate( );

            await _context.SaveChangesAsync( );

            return true;
        }

        

        public async Task<bool> DeactivateAsync (Guid ID)
        {
            var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.ID == ID && !x.IsDeleted);

            if ( category == null )
                return false;

            category.Deactivate( );

            await _context.SaveChangesAsync( );

            return true;
        }

        public async Task<bool> DeleteAsync (Guid ID)
        {
            var category = await _context.Categories
           .FirstOrDefaultAsync(x => x.ID == ID && !x.IsDeleted);

            if ( category == null )
                return false;

            category.Delete( );

            await _context.SaveChangesAsync( );

            return true;
        }

        private static CategoryDto ToDto (Category category)
        {
            return new CategoryDto
            {
                ID = category.ID,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ParentID = category.ParentID,
                IsActive = category.IsActive
            };
        }

        private static string? NormalizeSlug (string? slug)
        {
            if ( string.IsNullOrWhiteSpace(slug) )
                return null;

            return slug.Trim( ).ToLowerInvariant( );
        }
    }
}
