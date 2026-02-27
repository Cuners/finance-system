using Finance.Application.UseCases.Accounts;
using Finance.Application.UseCases.Accounts.CreateAccount;
using Finance.Application.UseCases.Accounts.CreateAccount.Request;
using Finance.Application.UseCases.Accounts.DeleteAccount;
using Finance.Application.UseCases.Accounts.DeleteAccount.Request;
using Finance.Application.UseCases.Accounts.GetAccountById;
using Finance.Application.UseCases.Accounts.GetAccountById.Request;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Request;
using Finance.Application.UseCases.Accounts.GetValueAccounts;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Request;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using Finance.Application.UseCases.Accounts.UpdateAccount.Request;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CreateAccountUseCase _createAccount;   
        private readonly GetAccountsByUserIdUseCase _getAccounts;
        private readonly GetValueAccountsUseCase _getAccountsValue;
        private readonly DeleteAccountUseCase _deleteAccount;
        private readonly GetAccountByIdUseCase _getAccountById;
        private readonly UpdateAccountUseCase _updateAccount;
        public AccountController(CreateAccountUseCase createAccount, 
                                 GetAccountsByUserIdUseCase getAccounts, 
                                 DeleteAccountUseCase deleteAccount, 
                                 GetAccountByIdUseCase getAccountById, 
                                 UpdateAccountUseCase updateAccount,
                                 GetValueAccountsUseCase getAccountsValue)
        {
            _createAccount = createAccount;
            _getAccounts = getAccounts;
            _getAccountById = getAccountById;
            _updateAccount = updateAccount;
            _deleteAccount = deleteAccount;
            _getAccountsValue=getAccountsValue;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetUserAccounts(CancellationToken ct)
        {
            //if (!Request.Cookies.TryGetValue("UserId", out var userIdString))
            //    return Unauthorized("User is not authenticated.");

            //if (!int.TryParse(userIdString, out var userId))
            //    return Unauthorized("Invalid user ID in cookies.");
            int userId = 1;
            var response = await _getAccounts.ExecuteAsync(new GetAccountsByUserIdRequest { UserId = userId },ct);

            return Ok(response);
        }
        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetValueAccounts(CancellationToken ct)
        {
            int userId = 1;
            var response = await _getAccountsValue.ExecuteAsync(new GetValueAccountsRequest { UserId = userId }, ct);
            return Ok(response);
        }
        [HttpGet("{accountid}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(int accountid, CancellationToken ct)
        {
            
            var request = new GetAccountByIdRequest { AccountId = accountid };
            var response = await _getAccountById.ExecuteAsync(request,ct);

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(CreateAccountRequest request, CancellationToken ct)
        {
            var response = await _createAccount.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpPut("{accountid}")]
        public async Task<ActionResult<AccountDto>> Update(UpdateAccountRequest request, CancellationToken ct)
        {
            var response = await _updateAccount.ExecuteAsync(request, ct);

            return Ok(response);
        }
        [HttpDelete("{accountid}")]
        public async Task<ActionResult> Delete(int accountid, CancellationToken ct)
        {
            var request = new DeleteAccountRequest{ AccountId = accountid };
            var response = await _deleteAccount.ExecuteAsync(request, ct);
            return Ok(response);
        }
    }
}
