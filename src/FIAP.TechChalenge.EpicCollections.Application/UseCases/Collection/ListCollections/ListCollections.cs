using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
public class ListCollections
    : IListCollections
{
    private readonly ICollectionRepository _collectionRepository;

    public ListCollections(ICollectionRepository collectionRepository)
    {
        _collectionRepository = collectionRepository;
    }

    public async Task<ListCollectionsOutput> Handle(
        ListCollectionsInput request,
        CancellationToken cancellationToken)
    {
        var searchOutput = await _collectionRepository.Search(
            new(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );
        return new ListCollectionsOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(CollectionModelOutput.FromCollection)
                .ToList()
        );
    }
}
