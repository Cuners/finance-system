using Finance.Application.Services;
using Finance.Application.UseCases.Transactions.CreateTransaction;
using Finance.Application.UseCases.Transactions.DeleteTransaction;
using Finance.Application.UseCases.Transactions.GetTransactionById;
using Finance.Application.UseCases.Transactions.GetTransactions;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary;
using Finance.Application.UseCases.Transactions.UpdateTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransactionServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ICreateTransactionUseCase _createTransaction;
        private readonly IDeleteTransactionUseCase _deleteTransaction;
        private readonly IGetTransactionsByAccountIdUseCase _getTransactionsByAccountId;
        private readonly IGetTransactionByIdUseCase _getTransactionById;
        private readonly IGetTransactionsUseCase _getTransactions;
        private readonly IGetTransactionsSummaryUseCase _getTransactionsSummary;
        private readonly IUpdateTransactionUseCase _updateTransaction;
        private readonly ICurrentUserService _currentUser;

        public TransactionController(
            ICreateTransactionUseCase createTransaction,
            IDeleteTransactionUseCase deleteTransaction,
            IGetTransactionsByAccountIdUseCase getTransactionsByAccountId,
            IGetTransactionByIdUseCase getTransactionById,
            IGetTransactionsUseCase getTransactions,
            IGetTransactionsSummaryUseCase getTransactionsSummary,
            IUpdateTransactionUseCase updateTransaction,
            ICurrentUserService currentUser)
        {
            _createTransaction = createTransaction;
            _getTransactionsByAccountId = getTransactionsByAccountId;
            _deleteTransaction = deleteTransaction;
            _getTransactionById = getTransactionById;
            _getTransactions = getTransactions;
            _getTransactionsSummary = getTransactionsSummary;
            _currentUser = currentUser;
            _updateTransaction = updateTransaction;
        }

        [HttpGet("Accounts/{accountId}/Transactions")]
        [Authorize]
        public async Task<ActionResult<GetTransactionsByAccountIdResult>> GetTransactionByAccountId(
            int accountId,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getTransactionsByAccountId.ExecuteAsync(
                new GetTransactionsByAccountIdQuery(accountId),
                userId,
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("{transactionid}")]
        [Authorize]
        public async Task<ActionResult<GetTransactionByIdResult>> GetTransactionById(
            int transactionid,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getTransactionById.ExecuteAsync(
                new GetTransactionByIdQuery(transactionid),
                userId,
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetTransactionsResult>> GetTransactions(
            string? type = null,
            int? accountId = null,
            string? sortOrder = null,
            string? sortBy = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            CancellationToken ct = default)
        {
            var userId = _currentUser.UserId;
            var result = await _getTransactions.ExecuteAsync(
                new GetTransactionsQuery(
                    AccountId: accountId,
                    Type: type,
                    SortOrder: sortOrder,
                    SortBy: sortBy,
                    StartDate: startDate,
                    EndDate: endDate),
                userId,
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("values")]
        [Authorize]
        public async Task<ActionResult<GetTransactionsSummaryResult>> GetTransactionsSummary(
            int year,
            int month,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getTransactionsSummary.ExecuteAsync(
                new GetTransactionsSummaryQuery(year, month),
                userId,
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPut("{transactionid}")]
        [Authorize]
        public async Task<ActionResult<UpdateTransactionResult>> Update(
            UpdateTransactionCommand command, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _updateTransaction.ExecuteAsync(command, userId, ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpDelete("{transactionid}")]
        [Authorize]
        public async Task<ActionResult<DeleteTransactionResult>> Delete(
            int transactionid,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _deleteTransaction.ExecuteAsync(
                new DeleteTransactionCommand(transactionid), 
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
        public async Task<ActionResult<CreateTransactionResult>> Create(
            CreateTransactionCommand command,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _createTransaction.ExecuteAsync(command, userId, ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
