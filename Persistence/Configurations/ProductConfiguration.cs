using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure (EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Name).HasMaxLength(500);
            builder.Property(x => x.Slug).HasMaxLength(250);
            builder.Property(x => x.SKU).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(50000);

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);

            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x=>x.IsDeleted).HasDefaultValue(false);

            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
