using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

public class CollectionPersistence
{
    private readonly EpicCollectionsDbContext _context;

    public CollectionPersistence(EpicCollectionsDbContext context)
        => _context = context;

    public async Task<DomainEntity.Collection.Collection?> GetById(Guid id)
        => await _context
            .Collections
            .Include(c => c.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task Insert(DomainEntity.Collection.Collection collection)
    {
        await _context.Collections.AddAsync(collection);
        await _context.SaveChangesAsync();
    }

    public async Task InsertList(List<DomainEntity.Collection.Collection> collections)
    {
        await _context.Collections.AddRangeAsync(collections);
        await _context.SaveChangesAsync();
    }

    public async Task Update(DomainEntity.Collection.Collection collection)
    {
        _context.Collections.Update(collection);
        await _context.SaveChangesAsync();
    }

    public async Task<DomainEntity.User?> GetUserByEmail(string email)
        => await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

    public async Task InsertUser(DomainEntity.User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<DomainEntity.Collection.CollectionItem?> GetItemById(Guid id)
    => await _context
        .CollectionItems
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddItemToCollection(DomainEntity.Collection.CollectionItem item)
    {
        await _context.CollectionItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }
}
