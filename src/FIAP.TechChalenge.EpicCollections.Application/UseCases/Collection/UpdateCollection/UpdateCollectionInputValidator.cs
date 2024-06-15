using FluentValidation;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
public class UpdateCollectionInputValidator
    : AbstractValidator<UpdateCollectionInput>
{
    public UpdateCollectionInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description must not be empty");
        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Category must be a valid value");
    }
}
