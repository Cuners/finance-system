using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Application.UseCases;
using Finance.Application.UseCases.Accounts.CreateAccount.Request;
using Finance.Application.UseCases.Accounts.CreateAccount.Response;
using Finance.Application.UseCases.Accounts.DeleteAccount.Request;
using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using Finance.Application.UseCases.Accounts.GetAccountById.Request;
using Finance.Application.UseCases.Accounts.GetAccountById.Response;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Request;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Request;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Response;
using Finance.Application.UseCases.Accounts.UpdateAccount.Request;
using Finance.Application.UseCases.Accounts.UpdateAccount.Response;
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
        private readonly IUseCase<CreateAccountRequest, CreateAccountResponse> _createAccount;
        private readonly IUseCase<GetAccountsByUserIdRequest, GetAccountsByUserIdResponse> _getAccounts;
        private readonly IUseCase<GetValueAccountsRequest, GetValueAccountsResponse> _getAccountsValue;
        private readonly IUseCase<DeleteAccountRequest, DeleteAccountResponse> _deleteAccount;
        private readonly IUseCase<GetAccountByIdRequest, GetAccountByIdResponse> _getAccountById;
        private readonly IUseCase<UpdateAccountRequest, UpdateAccountResponse> _updateAccount;
        private readonly ICurrentUserService _currentUser;
        public AccountController(IUseCase<CreateAccountRequest, CreateAccountResponse> createAccount,
                                 IUseCase<GetAccountsByUserIdRequest, GetAccountsByUserIdResponse> getAccounts,
                                 IUseCase<DeleteAccountRequest, DeleteAccountResponse> deleteAccount,
                                 IUseCase<GetAccountByIdRequest, GetAccountByIdResponse> getAccountById,
                                 IUseCase<UpdateAccountRequest, UpdateAccountResponse> updateAccount,
                                 IUseCase<GetValueAccountsRequest, GetValueAccountsResponse> getAccountsValue,
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
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetUserAccounts(CancellationToken ct)
        {
            int userId = _currentUser.UserId;
            var response = await _getAccounts.ExecuteAsync(new GetAccountsByUserIdRequest(),userId,ct);

            return Ok(response);
        }
        [HttpGet("balance")]
        [Authorize]
        public async Task<ActionResult<GetValueAccountsResponse>> GetValueAccounts(CancellationToken ct)
        {
            int userId = _currentUser.UserId;
            var response = await _getAccountsValue.ExecuteAsync(new GetValueAccountsRequest(),userId, ct);
            return Ok(response);
        }
        [HttpGet("{accountid}")]
        [Authorize]
        public async Task<ActionResult<GetAccountByIdResponse>> GetAccountById(int accountid, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var request = new GetAccountByIdRequest { AccountId = accountid};
            var response = await _getAccountById.ExecuteAsync(request,userId,ct);

            return Ok(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateAccountResponse>> Create(CreateAccountRequest request, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var response = await _createAccount.ExecuteAsync(request,userId,ct);
            return Ok(response);
        }
        [HttpPut("{accountid}")]
        [Authorize]
        public async Task<ActionResult<UpdateAccountResponse>> Update(UpdateAccountRequest request, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var response = await _updateAccount.ExecuteAsync(request,userId, ct);
            return Ok(response);
        }
        [HttpDelete("{accountid}")]
        [Authorize]
        public async Task<ActionResult<DeleteAccountResponse>> Delete(int accountid, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var request = new DeleteAccountRequest{ AccountId = accountid };
            var response = await _deleteAccount.ExecuteAsync(request,userId, ct);
            return Ok(response);
        }
    }
}
