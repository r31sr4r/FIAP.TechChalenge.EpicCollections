using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;

namespace FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Configurations
{
    internal class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasKey(collection => collection.Id);

            builder.Property(collection => collection.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(collection => collection.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(collection => collection.Category)
                .IsRequired();

            builder.Property(collection => collection.UserId)
                .IsRequired();

            builder.Property(collection => collection.CreatedAt)
                .IsRequired();

            builder.Property(collection => collection.UpdatedAt)
                .HasColumnType("datetime(6)");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(collection => collection.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey(ci => ci.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
