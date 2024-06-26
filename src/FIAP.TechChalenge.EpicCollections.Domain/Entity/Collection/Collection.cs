using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection
{
    public class Collection : AggregateRoot
    {
        private readonly List<CollectionItem> _items;

        public Collection(
            Guid userId,
            string name,
            string description,
            Category category) : base()
        {
            UserId = userId;
            Name = name;
            Description = description;
            Category = category;
            CreatedAt = DateTime.Now;
            _items = new List<CollectionItem>();

            Validate();
        }

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Category Category { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public IReadOnlyCollection<CollectionItem> Items => _items.AsReadOnly();

        public void AddItem(CollectionItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new EntityNotFoundException($"Item with id {itemId} not found");

            _items.Remove(item);
        }

        public void UpdateItem(Guid itemId, string name, string description, DateTime acquisitionDate, decimal value, string photoUrl)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new EntityNotFoundException($"Item with id {itemId} not found");

            item.Update(name, description, acquisitionDate, value, photoUrl);
        }

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
            if (!Enum.IsDefined(typeof(Category), Category))
                throw new EntityValidationException($"{nameof(Category)} must be a valid value");
        }
    }
}
