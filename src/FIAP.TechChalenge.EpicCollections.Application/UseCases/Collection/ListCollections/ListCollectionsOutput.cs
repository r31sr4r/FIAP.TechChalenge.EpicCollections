using FIAP.TechChalenge.EpicCollections.Application.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
public class ListCollectionsOutput
    : PaginatedListOutput<CollectionModelOutput>
{
    public ListCollectionsOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<CollectionModelOutput> items)
        : base(page, perPage, total, items)
    { }
}
