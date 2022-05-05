using MediatR;

namespace GBank.Api.Application.Accounts.Commands
{
    public class RegisterAccountCommand : IRequest<string>
    {
        public string CustomerId { get; set; }
        public decimal Balance { get; set; }
    }
}
