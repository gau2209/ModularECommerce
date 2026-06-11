using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Seed;

namespace Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure (EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired( );

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsSystemRole)
                .HasDefaultValue(false)
                .IsRequired( );

            builder.Property(x => x.CreatedAt)
                .IsRequired( );

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired( );

            builder.Property(x => x.DeletedAt);

            builder.HasIndex(x => x.Name)
                .IsUnique( );

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.RolePermissions)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(AuthSeedData.Roles);
        }
    }
}
