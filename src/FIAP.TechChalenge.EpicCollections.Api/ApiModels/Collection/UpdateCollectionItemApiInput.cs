namespace FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;

public class UpdateCollectionItemApiInput
{
    public UpdateCollectionItemApiInput(
        string name,
        string description,
        DateTime acquisitionDate,
        decimal value,
        string photoUrl
    )
    {
        Name = name;
        Description = description;
        AcquisitionDate = acquisitionDate;
        Value = value;
        PhotoUrl = photoUrl;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime AcquisitionDate { get; private set; }
    public decimal Value { get; private set; }
    public string PhotoUrl { get; private set; }
}
