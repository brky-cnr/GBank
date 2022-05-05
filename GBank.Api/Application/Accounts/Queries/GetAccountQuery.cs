using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace GBank.Api.Application.Accounts.Queries
{
    public class GetAccountQuery : IRequest<AccountDTO>
    {
        [FromRoute(Name = "accountId")]
        public string AccountId { get; set; }
    }
}