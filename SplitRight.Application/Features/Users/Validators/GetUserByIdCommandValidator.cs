using FluentValidation;
using SplitRight.Application.Features.Users.Commands;

namespace SplitRight.Application.Features.Users.Validators
{
    public class GetUserByIdCommandValidator : AbstractValidator<GetUserByIdCommand>
    {
        public GetUserByIdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id do Usuario deve ser informado.");
        }
    }
}
