using System.ComponentModel.DataAnnotations;

namespace Application.Categories.DTOs
{
    public class UpdateCategoryRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Slug { get; set; }
        public string? Description { get; set; }

        public Guid? ParentID { get; set; }

        public bool IsActive { get; set; }
    }
}
