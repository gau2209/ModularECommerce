using System.ComponentModel.DataAnnotations;

namespace Application.Categories.DTOs
{
    public class CreateCategoryRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Slug { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid? ParentID { get; set; }
    }
}
