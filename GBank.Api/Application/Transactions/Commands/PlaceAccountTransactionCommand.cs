using MediatR;

namespace GBank.Api.Application.Transactions.Commands
{
    public class PlaceAccountTransactionCommand : IRequest<string>
    {
        public string AccountId { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsDeposit { get; set; }

    }
}
