using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly EpicCollectionsDbContext _context;
        private DbSet<Collection> _collections => _context.Set<Collection>();
        private DbSet<CollectionItem> _collectionItems => _context.Set<CollectionItem>();
        public CollectionRepository(EpicCollectionsDbContext context)
        {
            _context = context;
        }

        public async Task Insert(Collection aggregate, CancellationToken cancellationToken)
        {
            await _collections.AddAsync(aggregate, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Collection> Get(Guid id, CancellationToken cancellationToken)
        {
            var collection = await _collections
                .Include(c => c.Items)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            NotFoundException.ThrowIfNull(collection, $"Collection with id {id} not found");
            return collection!;
        }

        public async Task Update(Collection aggregate, CancellationToken cancellationToken)
        {
            var existingEntity = await _collections
                .FirstOrDefaultAsync(x => x.Id == aggregate.Id, cancellationToken);

            if (existingEntity == null)
            {
                throw new DbUpdateConcurrencyException($"Collection with id {aggregate.Id} not found");
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task Delete(Collection aggregate, CancellationToken cancellationToken)
        {
            _collections.Remove(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<SearchOutput<Collection>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            var toSkip = (input.Page - 1) * input.PerPage;
            var query = _collections.AsNoTracking();
            query = AddSorting(query, input.OrderBy, input.Order);
            if (!string.IsNullOrWhiteSpace(input.Search))
                query = query.Where(x => x.Name.Contains(input.Search));

            var total = await query.CountAsync(cancellationToken);
            var items = await query.AsNoTracking()
                .Skip(toSkip)
                .Take(input.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<Collection>(
                currentPage: input.Page,
                perPage: input.PerPage,
                total: total,
                items: items
            );
        }

        private IQueryable<Collection> AddSorting(
            IQueryable<Collection> query,
            string orderProperty,
            SearchOrder order
        )
        {
            var orderedEnumerable = (orderProperty, order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
                ("createdAt", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
                ("createdAt", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.Name)
            };

            return orderedEnumerable
                .ThenBy(x => x.CreatedAt);
        }

        public async Task<IReadOnlyList<Collection>> GetCollectionsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _collections
                .Include(c => c.Items)
                .Where(c => c.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddItemToCollection(CollectionItem item, CancellationToken cancellationToken)
        {
            var collectionExists = await _collections
                .AsNoTracking()
                .AnyAsync(c => c.Id == item.CollectionId, cancellationToken);

            if (!collectionExists)
            {
                throw new EntityNotFoundException($"Collection with id {item.CollectionId} not found");
            }

            await _collectionItems.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
