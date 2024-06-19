using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;

public class UpdateCollectionApiInput
{
    public UpdateCollectionApiInput(
        string name,
        string description,
        Category category
    )
    {
        Name = name;
        Description = description;
        Category = category;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
}
