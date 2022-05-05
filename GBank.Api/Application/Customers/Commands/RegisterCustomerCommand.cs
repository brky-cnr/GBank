using MediatR;

namespace GBank.Api.Application.Customers.Commands
{
    public class RegisterCustomerCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
