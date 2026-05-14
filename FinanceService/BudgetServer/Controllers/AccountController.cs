using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Application.UseCases;
using Finance.Application.UseCases.Accounts.CreateAccount;
using Finance.Application.UseCases.Accounts.DeleteAccount;
using Finance.Application.UseCases.Accounts.GetAccountById;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId;
using Finance.Application.UseCases.Accounts.GetValueAccounts;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICreateAccountUseCase _createAccount;
        private readonly IGetAccountsByUserIdUseCase _getAccounts;
        private readonly IGetValueAccountsUseCase _getAccountsValue;
        private readonly IDeleteAccountUseCase _deleteAccount;
        private readonly IGetAccountByIdUseCase _getAccountById;
        private readonly IUpdateAccountUseCase _updateAccount;
        private readonly ICurrentUserService _currentUser;
        public AccountController(ICreateAccountUseCase createAccount,
                                 IGetAccountsByUserIdUseCase getAccounts,
                                 IDeleteAccountUseCase deleteAccount,
                                 IGetAccountByIdUseCase getAccountById,
                                 IUpdateAccountUseCase updateAccount,
                                 IGetValueAccountsUseCase getAccountsValue,
                                 ICurrentUserService currentUser)
        {
            _createAccount = createAccount;
            _getAccounts = getAccounts;
            _getAccountById = getAccountById;
            _updateAccount = updateAccount;
            _deleteAccount = deleteAccount;
            _getAccountsValue = getAccountsValue;
            _currentUser= currentUser;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<AccountSummaryDto>>> GetUserAccounts(CancellationToken ct)
        {
            int userId = _currentUser.UserId;
            var result = await _getAccounts.ExecuteAsync(
                new GetAccountsByUserIdQuery(), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpGet("balance")]
        [Authorize]
        public async Task<ActionResult<decimal>> GetValueAccounts(CancellationToken ct)
        {
            int userId = _currentUser.UserId;
            var result = await _getAccountsValue.ExecuteAsync(
                new GetValueAccountsQuery(), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
        [HttpGet("{accountid}")]
        [Authorize]
        public async Task<ActionResult<AccountDto>> GetAccountById(
            int accountid, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getAccountById.ExecuteAsync(
                new GetAccountByIdQuery(accountid), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateAccountResult>> Create(
            CreateAccountCommand command, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _createAccount.ExecuteAsync(command,userId,ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpPut("{accountid}")]
        [Authorize]
        public async Task<ActionResult<UpdateAccountResult>> Update( 
            UpdateAccountCommand command, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _updateAccount.ExecuteAsync(command, userId, ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [HttpDelete("{accountid}")]
        [Authorize]
        public async Task<ActionResult<DeleteAccountResult>> Delete(
            int accountid, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _deleteAccount.ExecuteAsync(
                new DeleteAccountCommand(accountid), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
