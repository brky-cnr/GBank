using MediatR;
using GBank.Domain.Documents;
using GBank.Domain.Events;
using GBank.Domain.Exceptions;
using GBank.Domain.Interfaces;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GBank.Api.Application.Customers.Commands
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, string>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;
        public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
        }

        public async Task<string> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new RegisterCustomerCommandValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException(message, HttpStatusCode.BadRequest);
            }

            var customer = new Customer
            {
                Email = request.Email,
                Name = request.Name,
                Address = request.Address
            };

            var customerId = await _customerRepository.CreateCustomerAsync(customer);

            _ = _mediator.Publish(new CustomerCreatedEvent
            {
                CustomerId = customerId
            }, cancellationToken);

            return customerId;
        }
    }
}
