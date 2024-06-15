using FluentValidation;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;
public class GetCollectionInputValidator
    : AbstractValidator<GetCollectionInput>
{
    public GetCollectionInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
