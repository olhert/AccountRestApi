using AccountRestApi.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace AccountRestApi.Controllers
{
    [ApiController]
    [Route("apisecure/[controller]")]
    public class BalanceOperationsController : ControllerBase
    {
        private readonly IAccountsStore _accountsStore;

        public BalanceOperationsController(IAccountsStore accountsStore)
        {
            _accountsStore = accountsStore;
        }

        [HttpPost("/accounts/deposit/{accountId}/{sumOfDeposit:double}")]
        public IActionResult Deposit(string accountId, double sumOfDeposit)
        {
            return Ok(_accountsStore.Deposit(accountId, sumOfDeposit));
        }
        
        [HttpPost("/accounts/withdraw/{accountId}/{sumOfWithdrawal:double}")]
        public IActionResult Withdrawal(string accountId, double sumOfWithdrawal)
        {
            return Ok(_accountsStore.Withdrawal(accountId, sumOfWithdrawal));
        }
        
        [HttpPost("/accounts/transfer/{senderAccId}/{recipientAccId}/{sumOfTransfer:double}")]
        public IActionResult Transfer(string senderAccId, string recipientAccId, double sumOfTransfer)
        {
            _accountsStore.Transfer(senderAccId, recipientAccId, sumOfTransfer);
            return Ok(new StatusModel
            {
                Status = "success"
            });
        }
    }
    
}