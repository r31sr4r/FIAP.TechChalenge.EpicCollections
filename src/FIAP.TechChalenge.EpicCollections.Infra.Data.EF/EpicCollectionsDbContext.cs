using Microsoft.EntityFrameworkCore;
using FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Configurations;

namespace FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
public class EpicCollectionsDbContext
    : DbContext
{
    public DbSet<User> Users => Set<User>();

    public EpicCollectionsDbContext(
        DbContextOptions<EpicCollectionsDbContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
