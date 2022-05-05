using FluentValidation;
using MongoDB.Bson;

namespace GBank.Api.Application.Accounts.Commands
{
    public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Customer Id should not be empty.")
                .Must(x => ObjectId.TryParse(x, out _)).WithMessage("Invalid customer id!");
            RuleFor(x => x.Balance)
                .NotNull().WithMessage("Balance should not be empty.")
                .GreaterThan(0).WithMessage("Balance should be more than 0.");
        }
    }
}
