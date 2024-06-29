using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Configurations;
internal class CollectionItemConfiguration : IEntityTypeConfiguration<CollectionItem>
{
    public void Configure(EntityTypeBuilder<CollectionItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(item => item.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(item => item.AcquisitionDate)
            .IsRequired();

        builder.Property(item => item.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(item => item.PhotoUrl)
            .HasMaxLength(2048);
    }
}
