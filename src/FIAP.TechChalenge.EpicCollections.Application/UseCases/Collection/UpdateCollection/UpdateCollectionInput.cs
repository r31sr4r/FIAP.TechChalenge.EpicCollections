using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
public class UpdateCollectionInput : IRequest<CollectionModelOutput>
{
    public UpdateCollectionInput(
        Guid id,
        string name,
        string description,
        Category category,
        Guid userId,
        bool isActive = true        
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
        UserId = userId;
        IsActive = isActive;

    }

public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }

}
