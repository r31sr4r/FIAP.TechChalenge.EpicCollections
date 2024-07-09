using FIAP.TechChalenge.EpicCollections.Application.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
public class ListCollectionItemsOutput
    : PaginatedListOutput<CollectionModelOutput>
{
    public ListCollectionItemsOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<CollectionModelOutput> items)
        : base(page, perPage, total, items)
    { }
}
