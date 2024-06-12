using FluentValidation;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.GetUser;
public class GetUserInputValidator
    : AbstractValidator<GetUserInput>
{
    public GetUserInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
