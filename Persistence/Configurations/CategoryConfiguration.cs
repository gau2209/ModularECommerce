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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure (EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired( );

            builder.Property(x => x.Slug)
                .HasMaxLength(250)
                .IsRequired( );

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .IsRequired( )
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .IsRequired( );

            builder.Property(x => x.UpdatedAt);

            builder.HasIndex(x => x.Slug)
                .IsUnique( );

            builder.HasIndex(x => x.Name);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
