
using Finance.Application.UseCases.Transactions;
using Finance.Application.UseCases.Transactions.CreateTransaction;
using Finance.Application.UseCases.Transactions.CreateTransaction.Request;
using Finance.Application.UseCases.Transactions.DeleteTransaction;
using Finance.Application.UseCases.Transactions.DeleteTransaction.Request;
using Finance.Application.UseCases.Transactions.GetTransactionById;
using Finance.Application.UseCases.Transactions.GetTransactionById.Request;
using Finance.Application.UseCases.Transactions.GetTransactions;
using Finance.Application.UseCases.Transactions.GetTransactions.Request;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Request;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary.Request;
using Microsoft.AspNetCore.Mvc;

namespace TransactionServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly CreateTransactionUseCase _createTransaction;
       // private readonly GetTransactionsByUserIdUseCase _getTransactions;
        private readonly DeleteTransactionUseCase _deleteTransaction;
        private readonly GetTransactionsByAccountIdUseCase _getTransactionByAccountId;
        private readonly GetTransactionByIdUseCase _getTransactionById;
        private readonly GetTransactionsUseCase _getTransactions;
        private readonly GetTransactionsSummaryUseCase _getTransactionsSummary;
        public TransactionController(CreateTransactionUseCase createTransaction, 
                                     DeleteTransactionUseCase deleteTransaction, 
                                     GetTransactionsByAccountIdUseCase getTransactionByAccountId, 
                                     GetTransactionByIdUseCase getTransactionById,
                                     GetTransactionsUseCase getTransactions, 
                                     GetTransactionsSummaryUseCase getTransactionsSummary)
        {
            _createTransaction = createTransaction;
            _getTransactionByAccountId = getTransactionByAccountId;
            _deleteTransaction = deleteTransaction;
            _getTransactionById = getTransactionById;
            _getTransactions = getTransactions;
            _getTransactionsSummary = getTransactionsSummary;
        }
        [HttpGet("Accounts/{accountId}/Transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionByAccountId(int accountId, CancellationToken ct)
        {
            var request = new GetTransactionsByAccountIdRequest { AccountId = accountId };
            var response = await _getTransactionByAccountId.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpGet("{transactionid}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(int transactionid, CancellationToken ct)
        {
            var request = new GetTransactionByIdRequest { TransactionId = transactionid };
            var response = await _getTransactionById.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(string? type=null, 
                                                                                     string? sortOrder=null, 
                                                                                     string? sortBy=null, 
                                                                                     DateOnly? startDate=null, 
                                                                                     DateOnly? endDate=null, 
                                                                                     CancellationToken ct=default)
        {
            var request = new GetTransactionsRequest(
                            UserId: 1,
                            Type: type,
                            SortOrder: sortOrder,
                            SortBy:sortBy,
                            StartDate: startDate,
                            EndDate: endDate
                        );
            var response = await _getTransactions.ExecuteAsync(request, ct);
            return Ok(response);
        }
        [HttpGet("values")]
        public async Task<ActionResult<TransactionSummaryDto>> GetTransactionsSummary(int year,int month,CancellationToken ct)
        {
            var request = new  GetTransactionsSummaryRequest { UserId = 1, Year=year, Month=month };
            var response = await _getTransactionsSummary.ExecuteAsync(request, ct);
            return Ok(response);
        }

        [HttpDelete("{transactionid}")]
        public async Task<ActionResult> Delete(int transactionid, CancellationToken ct)
        {
            var request = new DeleteTransactionRequest { TransactionId = transactionid };
            var response = await _deleteTransaction.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(CreateTransactionRequest request, CancellationToken ct)
        {
            var response = await _createTransaction.ExecuteAsync(request,ct);
            return Ok(response);
        }
    }
}
