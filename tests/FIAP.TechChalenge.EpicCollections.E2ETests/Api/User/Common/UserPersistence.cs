using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;


namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.User.Common;
public class UserPersistence
{
    private readonly EpicCollectionsDbContext _context;

    public UserPersistence(EpicCollectionsDbContext context)
        => _context = context;

    public async Task<DomainEntity.User?> GetById(Guid id)
        => await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task InsertList(List<DomainEntity.User> users)
    {
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}
