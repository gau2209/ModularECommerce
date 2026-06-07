using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure (EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Token)
                .HasMaxLength(500)
                .IsRequired( );

            builder.Property(x => x.ExpiresAt)
                .IsRequired( );

            builder.Property(x => x.RevokedAt);

            builder.Property(x => x.CreatedByIp)
                .HasMaxLength(100);

            builder.Property(x => x.RevokedByIp)
                .HasMaxLength(100);

            builder.Property(x => x.ReplacedByToken)
                .HasMaxLength(500);

            builder.Property(x => x.ReasonRevoked)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired( );

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired( );

            builder.Property(x => x.DeletedAt);

            builder.HasIndex(x => x.Token)
                .IsUnique( );

            builder.HasIndex(x => x.UserId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
