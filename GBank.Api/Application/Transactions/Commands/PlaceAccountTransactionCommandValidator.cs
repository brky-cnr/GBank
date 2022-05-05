using FluentValidation;
using MongoDB.Bson;

namespace GBank.Api.Application.Transactions.Commands
{
    public class PlaceAccountTransactionCommandValidator : AbstractValidator<PlaceAccountTransactionCommand>
    {
        public PlaceAccountTransactionCommandValidator()
        {
            RuleFor(x => x.AccountId)
                .NotNull().WithMessage("Account id should not be empty.")
                .Must(x => ObjectId.TryParse(x, out _)).WithMessage("Invalid account id!");
            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Customer id should not be empty.")
                .Must(x => ObjectId.TryParse(x, out _)).WithMessage("Invalid customer id!");
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount should not be empty.")
                .GreaterThan(0).WithMessage("Amount should be more than 0.");
            RuleFor(x => x.Description).NotNull().WithMessage("Description should not be empty.");
            RuleFor(x => x.IsDeposit).NotNull().WithMessage("IsDeposit should not be empty.");
        }
    }
}
