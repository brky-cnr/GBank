using FluentValidation;
using MongoDB.Bson;

namespace GBank.Api.Application.Transactions.Queries
{
    public class GetCustomerTransactionsQueryValidator : AbstractValidator<GetCustomerTransactionsQuery>
    {
        public GetCustomerTransactionsQueryValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Customer id should not be empty.")
                .Must(x => ObjectId.TryParse(x, out _)).WithMessage("Invalid customer id!");
        }
    }
}