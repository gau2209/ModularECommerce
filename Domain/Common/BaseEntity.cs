using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid ID { get; protected set; }

        public DateTime CreatedAt { get; protected set; } = DateTime.Now;
        public string? CreatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedAt { get; protected set; } = DateTime.Now;
        public string? UpdatedBy { get; set; } = string.Empty;

        public bool IsDeleted { get; protected set; }

        public DateTime? DeletedAt { get; protected set; }


        public void MarkUpdated ()
        {
            UpdatedAt = DateTime.Now;
        }

        public void SoftDelete ()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }

    }
}
