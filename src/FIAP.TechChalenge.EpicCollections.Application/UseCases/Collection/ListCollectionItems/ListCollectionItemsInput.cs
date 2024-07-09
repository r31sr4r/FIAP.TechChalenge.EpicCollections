using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.Common;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
public class ListCollectionItemsInput
    : PaginatedListInput,
    IRequest<CollectionModelOutput>
{
    public ListCollectionItemsInput(
        Guid collectionId,
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc)
        : base(page, perPage, search, sort, dir)
    {
        CollectionId = collectionId;
    }

    public Guid CollectionId { get; set; }
}
