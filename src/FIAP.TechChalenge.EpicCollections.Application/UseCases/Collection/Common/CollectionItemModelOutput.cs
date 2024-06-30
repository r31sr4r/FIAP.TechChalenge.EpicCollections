using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common
{
    public class CollectionItemModelOutput
    {
        public CollectionItemModelOutput(
            Guid id,
            Guid collectionId,
            string name,
            string description,
            DateTime acquisitionDate,
            decimal value,
            string photoUrl)
        {
            Id = id;
            CollectionId = collectionId;
            Name = name;
            Description = description;
            AcquisitionDate = acquisitionDate;
            Value = value;
            PhotoUrl = photoUrl;
        }

        public Guid Id { get; private set; }
        public Guid CollectionId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime AcquisitionDate { get; private set; }
        public decimal Value { get; private set; }
        public string PhotoUrl { get; private set; }

        public static CollectionItemModelOutput FromCollectionItem(CollectionItem item)
        {
            return new CollectionItemModelOutput(
                item.Id,
                item.CollectionId,
                item.Name,
                item.Description,
                item.AcquisitionDate,
                item.Value,
                item.PhotoUrl
            );
        }
    }
}
