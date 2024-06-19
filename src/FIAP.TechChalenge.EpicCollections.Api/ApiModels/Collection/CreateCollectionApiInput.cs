using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;

public class CreateCollectionApiInput
{
    public CreateCollectionApiInput(
        string name,
        string description,
        Category category
    )
    {
        Name = name;
        Description = description;
        Category = category;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
}
