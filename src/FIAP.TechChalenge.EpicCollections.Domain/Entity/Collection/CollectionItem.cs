using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using EntityBase = FIAP.TechChalenge.EpicCollections.Domain.SeedWork;

namespace FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection
{
    public class CollectionItem : EntityBase.Entity
    {
        public CollectionItem(
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

            Validate();
        }

        public Guid CollectionId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime AcquisitionDate { get; private set; }
        public decimal Value { get; private set; }
        public string PhotoUrl { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
            if (Name.Length <= 3)
                throw new EntityValidationException($"{nameof(Name)} should be greater than 3 characters");
            if (Name.Length >= 255)
                throw new EntityValidationException($"{nameof(Name)} should be less than 255 characters");
            if (Value < 0)
                throw new EntityValidationException($"{nameof(Value)} should not be negative");
        }

        public void Update(string name, string description, DateTime acquisitionDate, decimal value, string photoUrl)
        {
            Name = name;
            Description = description;
            AcquisitionDate = acquisitionDate;
            Value = value;
            PhotoUrl = photoUrl;

            Validate();
        }
    }
}
