using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using MediatR;
using System;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
public class UpdateCollectionItemInput : IRequest<CollectionItemModelOutput>
{
    public UpdateCollectionItemInput(
        Guid collectionId,
        Guid itemId,
        string name,
        string description,
        DateTime acquisitionDate,
        decimal value,
        string photoUrl)
    {
        CollectionId = collectionId;
        ItemId = itemId;
        Name = name;
        Description = description;
        AcquisitionDate = acquisitionDate;
        Value = value;
        PhotoUrl = photoUrl;
    }

    public Guid CollectionId { get; set; }
    public Guid ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public decimal Value { get; set; }
    public string PhotoUrl { get; set; }
}
