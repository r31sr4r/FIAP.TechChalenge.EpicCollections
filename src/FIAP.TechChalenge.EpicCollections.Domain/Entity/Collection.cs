using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork;

namespace FIAP.TechChalenge.EpicCollections.Domain.Entity
{
    public class Collection : AggregateRoot
    {
        public Collection()
        {
        }

        public Collection(
            Guid userId,
            string name,
            string description,
            Category category
        ) : base()
        {
            UserId = userId;
            Name = name;
            Description = description;
            Category = category;
            CreatedAt = DateTime.Now;

            Validate();
        }

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Category Category { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public void Update(string name, string description, Category category)
        {
            Name = name;
            Description = description;
            Category = category;
            UpdatedAt = DateTime.Now;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
            if (Name.Length <= 3)
                throw new EntityValidationException($"{nameof(Name)} should be greater than 3 characters");
            if (Name.Length >= 255)
                throw new EntityValidationException($"{nameof(Name)} should be less than 255 characters");
            if (string.IsNullOrWhiteSpace(Description))
                throw new EntityValidationException($"{nameof(Description)} should not be empty or null");
        }
    }
}
