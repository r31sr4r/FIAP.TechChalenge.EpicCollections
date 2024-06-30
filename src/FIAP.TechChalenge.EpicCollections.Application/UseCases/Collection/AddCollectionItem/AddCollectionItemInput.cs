using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

public class AddCollectionItemInput : IRequest<CollectionItemModelOutput>
{
    public AddCollectionItemInput(
        Guid collectionId,
        string name,
        string description,
        DateTime acquisitionDate,
        decimal value,
        string photoUrl = null)
    {
        CollectionId = collectionId;
        Name = name;
        Description = description;
        AcquisitionDate = acquisitionDate;
        Value = value;
        PhotoUrl = photoUrl;
    }

    public Guid CollectionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public decimal Value { get; set; }
    public string PhotoUrl { get; set; }
}
