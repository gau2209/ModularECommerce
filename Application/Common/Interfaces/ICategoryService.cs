using Application.Categories.DTOs;

namespace Application.Common.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetPublicCategoriesAsync ();
        Task<List<CategoryDto>> GetAdminCategoriesAsync ();
        Task<CategoryDto?> GetByIDAsync (Guid ID);
        Task<CategoryDto> CreateAsync (CreateCategoryRequest request);
        Task<CategoryDto?> UpdateAsync (Guid ID, UpdateCategoryRequest request);
        Task<bool> DeleteAsync (Guid ID);
        Task<bool> ActivateAsync (Guid ID);
        Task<bool> DeactivateAsync (Guid ID);
    }
}
