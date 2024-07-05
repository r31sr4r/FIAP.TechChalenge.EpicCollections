using FluentValidation;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem
{
    public class GetCollectionItemInputValidator : AbstractValidator<GetCollectionItemInput>
    {
        public GetCollectionItemInputValidator()
        {
            RuleFor(x => x.CollectionId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}
