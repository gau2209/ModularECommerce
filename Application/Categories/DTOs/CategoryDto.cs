using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.DTOs
{
    public class CategoryDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public Guid? ParentID { get; set; }
        public bool IsActive { get; set; }
    }
}
