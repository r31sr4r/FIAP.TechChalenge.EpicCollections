using FluentValidation;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
public class UpdateCollectionItemInputValidator : AbstractValidator<UpdateCollectionItemInput>
{
    public UpdateCollectionItemInputValidator()
    {
        RuleFor(x => x.CollectionId)
            .NotEmpty()
            .WithMessage("CollectionId must not be empty");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId must not be empty");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description must not be empty");

        RuleFor(x => x.AcquisitionDate)
            .NotEmpty()
            .WithMessage("AcquisitionDate must not be empty");

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("Value must be greater than zero");
    }
}
