using FluentValidation;
using MongoDB.Bson;

namespace GBank.Api.Application.Accounts.Queries
{
    public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
    {
        public GetAccountQueryValidator()
        {
            RuleFor(x => x.AccountId)
                .NotNull().WithMessage("Account id should not be empty.")
                .Must(x => ObjectId.TryParse(x, out _)).WithMessage("Invalid account id!");
        }
    }
}