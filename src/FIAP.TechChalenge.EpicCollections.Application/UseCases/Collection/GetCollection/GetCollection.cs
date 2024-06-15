using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;
public class GetCollection : IGetCollection
{
    private readonly ICollectionRepository _repository;

    public GetCollection(ICollectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<CollectionModelOutput> Handle(
        GetCollectionInput request,
        CancellationToken cancellationToken
        )
    {
        var collection = await _repository.Get(request.Id, cancellationToken);
        return CollectionModelOutput.FromCollection(collection);
    }
}
