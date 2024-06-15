using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.Common;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
public class ListCollectionsInput
    : PaginatedListInput,
    IRequest<ListCollectionsOutput>
{
    public ListCollectionsInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc)
        : base(page, perPage, search, sort, dir)
    { }

    public ListCollectionsInput()
        : base(1, 15, "", "", SearchOrder.Asc)
    { }
}
