using FluentValidation;
using MongoDB.Bson;

namespace GBank.Api.Application.Transactions.Queries
{
    public class GetCustomerTransactionsByDateQueryValidator : AbstractValidator<GetCustomerTransactionsByDateQuery>
    {
        public GetCustomerTransactionsByDateQueryValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Customer Id should not be empty.")
                .Must(x => ObjectId.TryParse(x, out _)).WithMessage("Invalid customer id!");
            RuleFor(x => x.StartDate).NotNull().WithMessage("Start Date should not be empty.");
            RuleFor(x => x.EndDate).NotNull().WithMessage("End Date should not be empty.");
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("Start Date should be less than to End Date.");
        }
    }
}
