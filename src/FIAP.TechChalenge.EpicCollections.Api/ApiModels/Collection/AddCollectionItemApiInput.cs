namespace FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection
{
    public class AddCollectionItemApiInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal Value { get; set; }
        public string PhotoUrl { get; set; }
    }
}
