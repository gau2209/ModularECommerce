using Application.Common.Models;
using Application.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetPublicProductsAsync ();
        Task<List<ProductDto>> GetAdminProductsAsync ();
        Task<ProductDto?> GetByIDAsync (Guid id);
        Task<ProductDto> CreateAsync (CreateProductRequest request);
        Task<ProductDto?> UpdateAsync (Guid id, UpdateProductRequest request);
        Task<bool> DeleteAsync (Guid id);
        Task<bool> ActivateAsync (Guid id);
        Task<bool> DeactivateAsync (Guid id);
        Task<PagedResult<ProductDto>> SearchAsync (ProductSearchRequest request);
    }
}
