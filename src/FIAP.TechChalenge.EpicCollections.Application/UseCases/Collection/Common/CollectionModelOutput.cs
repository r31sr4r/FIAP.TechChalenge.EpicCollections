using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
public class CollectionModelOutput
{
    public CollectionModelOutput(
        Guid id,
        Guid userId,
        string name,
        string description,
        Category category,
        DateTime createdAt,
        List<CollectionItemModelOutput> items
    )
    {
        Id = id;
        UserId = userId;
        Name = name;
        Description = description;
        Category = category;
        CreatedAt = createdAt;
        Items = items;
    }

    public Guid Id { get; set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<CollectionItemModelOutput> Items { get; private set; }

    public static CollectionModelOutput FromCollection(Domain.Entity.Collection.Collection collection)
    {
        var items = collection.Items
            .Select(CollectionItemModelOutput.FromCollectionItem)
            .ToList();

        return new CollectionModelOutput(
            collection.Id,
            collection.UserId,
            collection.Name,
            collection.Description,
            collection.Category,
            collection.CreatedAt,
            items
        );
    }
}
