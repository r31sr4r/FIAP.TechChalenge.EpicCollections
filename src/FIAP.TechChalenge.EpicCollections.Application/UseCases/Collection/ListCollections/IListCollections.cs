using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
public interface IListCollections
    : IRequestHandler<ListCollectionsInput, ListCollectionsOutput>
{
}
