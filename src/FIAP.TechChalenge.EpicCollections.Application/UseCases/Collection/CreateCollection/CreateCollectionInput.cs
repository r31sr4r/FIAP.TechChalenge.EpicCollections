using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
public class CreateCollectionInput : IRequest<CollectionModelOutput>
{
    public CreateCollectionInput(
        Guid userId,
        string name,
        string description,
        Category category
    )
    {
        UserId = userId;
        Name = name;
        Description = description;
        Category = category;
    }

    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
}
