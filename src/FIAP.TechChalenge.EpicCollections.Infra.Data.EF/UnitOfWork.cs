using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;

namespace FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
public class UnitOfWork
    : IUnitOfWork
{
    private readonly EpicCollectionsDbContext _context;

    public UnitOfWork(EpicCollectionsDbContext context)
    {
        _context = context;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
