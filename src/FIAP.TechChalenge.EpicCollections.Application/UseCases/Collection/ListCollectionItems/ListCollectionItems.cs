using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
public class ListCollectionItems : IListCollectionItems
{
    private readonly ICollectionRepository _collectionRepository;

    public ListCollectionItems(ICollectionRepository collectionRepository)
    {
        _collectionRepository = collectionRepository;
    }

    public async Task<CollectionModelOutput> Handle(ListCollectionItemsInput request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.GetCollectionWithItems(request.CollectionId, cancellationToken);

        if (collection == null)
            throw new NotFoundException($"Collection with id {request.CollectionId} not found");

        return CollectionModelOutput.FromCollection(collection);
    }
}
