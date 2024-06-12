using FluentValidation;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Update;
public class UpdateUserInputValidator
    : AbstractValidator<UpdateUserInput>
{
    public UpdateUserInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty");
    }
}
