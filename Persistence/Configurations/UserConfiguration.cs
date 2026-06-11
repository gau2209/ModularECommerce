using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure (EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Email)
                .HasMaxLength(256)
                .IsRequired( );

            builder.Property(x => x.UserName)
                .HasMaxLength(100)
                .IsRequired( );

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .IsRequired( );

            builder.Property(x => x.FullName)
                .HasMaxLength(200)
                .IsRequired( );

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(30);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .IsRequired( );

            builder.Property(x => x.CreatedAt)
                .IsRequired( );

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired( );

            builder.Property(x => x.DeletedAt);

            builder.HasIndex(x => x.Email)
                .IsUnique( );

            builder.HasIndex(x => x.UserName)
                .IsUnique( );

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.RefreshTokens)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
