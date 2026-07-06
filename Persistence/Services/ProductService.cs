using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Products.DTOs;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public ProductService (AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<ProductDto>> GetPublicProductsAsync ()
        {
            return await _context.Products
                .AsNoTracking( )
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted && x.IsActive && !x.Category.IsDeleted && x.Category.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => ToDto(x))
                .ToListAsync( );
        }

        public async Task<List<ProductDto>> GetAdminProductsAsync ()
        {
            return await _context.Products
                .AsNoTracking( )
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => ToDto(x))
                .ToListAsync( );
        }

        public async Task<ProductDto?> GetByIDAsync (Guid id)
        {
            var product = await _context.Products
                .AsNoTracking( )
                .Include(x => x.Category)
                .Where(x => x.ID == id && !x.IsDeleted)
                .Select(x => ToDto(x))
                .FirstOrDefaultAsync( );

            return product;
        }

        public async Task<ProductDto> CreateAsync (CreateProductRequest request)
        {
            var name = request.Name.Trim( );
            var slug = request.Slug.Trim( ).ToLowerInvariant( );
            var sku = request.SKU.Trim( ).ToUpperInvariant( );

            await ValidateCategoryExistsAsync(request.CategoryID);
            await ValidateSlugUniqueAsync(slug);
            await ValidateSkuUniqueAsync(sku);

            var product = new Product(
                name: name,
                slug: slug,
                sku: sku,
                price: request.Price,
                categoryID: request.CategoryID,
                description: request.Description);

            _context.Products.Add(product);

            await _context.SaveChangesAsync( );

            return await GetByIDAsync(product.ID)
                ?? ToDto(product);
        }

        public async Task<ProductDto?> UpdateAsync (Guid id, UpdateProductRequest request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted);

            if ( product == null )
                return null;

            var name = request.Name.Trim( );
            var slug = request.Slug.Trim( ).ToLowerInvariant( );
            var sku = request.SKU.Trim( ).ToUpperInvariant( );

            await ValidateCategoryExistsAsync(request.CategoryID);
            await ValidateSlugUniqueAsync(slug, id);
            await ValidateSkuUniqueAsync(sku, id);

            product.Update(
                name: name,
                slug: slug,
                sku: sku,
                price: request.Price,
                categoryID: request.CategoryID,
                description: request.Description,
                isActive: request.IsActive);

            await _context.SaveChangesAsync( );

            return await GetByIDAsync(product.ID);
        }

        public async Task<bool> DeleteAsync (Guid id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted);

            if ( product == null )
                return false;

            product.SoftDelete( );

            await _context.SaveChangesAsync( );

            return true;
        }
        public async Task<bool> ActivateAsync (Guid id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted);

            if ( product == null )
                return false;

            product.Activate( );

            await _context.SaveChangesAsync( );

            return true;
        }

        public async Task<bool> DeactivateAsync (Guid id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted);

            if ( product == null )
                return false;

            product.Deactivate( );

            await _context.SaveChangesAsync( );

            return true;
        }

        public async Task<PagedResult<ProductDto>> SearchAsync (ProductSearchRequest request)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
               await sqlConnection.OpenAsync( );

                var param = new DynamicParameters( );
                param.Add("@Name", request?.Keyword);
                param.Add("@IsActive", request?.IsActive);
                    
                var Query = await sqlConnection.QueryAsync<Product>("Product_Get",param,commandType: System.Data.CommandType.StoredProcedure,commandTimeout:240);

                if(Query == null || !Query.Any())
                    return new PagedResult<ProductDto>();

                var QuerytoDTO = Query.Select(x=> ToDto(x)).ToList();
                var totalCount = QuerytoDTO.Count;
                return new PagedResult<ProductDto>
                {
                    Items = QuerytoDTO.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                };
            }
        }

        #region Helper

        private async Task ValidateCategoryExistsAsync (Guid categoryID)
        {
            var exists = await _context.Categories
                .AnyAsync(x => x.ID == categoryID && !x.IsDeleted && x.IsActive);

            if ( !exists )
                throw new InvalidOperationException("Category does not exist or is inactive.");
        }

        private async Task ValidateSlugUniqueAsync (string slug, Guid? currentProductID = null)
        {
            var exists = await _context.Products
                .AnyAsync(x =>
                    !x.IsDeleted &&
                    x.Slug == slug &&
                    ( !currentProductID.HasValue || x.ID != currentProductID.Value ));

            if ( exists )
                throw new InvalidOperationException("Product slug already exists.");
        }

        private async Task ValidateSkuUniqueAsync (string sku, Guid? currentProductID = null)
        {
            var exists = await _context.Products
                .AnyAsync(x =>
                    !x.IsDeleted &&
                    x.SKU == sku &&
                    ( !currentProductID.HasValue || x.ID != currentProductID.Value ));

            if ( exists )
                throw new InvalidOperationException("Product SKU already exists.");
        }
        private static ProductDto ToDto (Product product)
        {
            return new ProductDto
            {
                ID = product.ID,
                Name = product.Name,
                Slug = product.Slug,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.CategoryID,
                CategoryName = product.Category?.Name,
                IsActive = product.IsActive
            };
        }

        #endregion
    }
}
