using Finance.Application.UseCases;
using Finance.Application.UseCases.Transactions.CreateTransaction.Request;
using Finance.Application.UseCases.Transactions.CreateTransaction.Response;
using Finance.Application.UseCases.Transactions.DeleteTransaction.Request;
using Finance.Application.UseCases.Transactions.DeleteTransaction.Response;
using Finance.Application.UseCases.Transactions.GetTransactionById.Request;
using Finance.Application.UseCases.Transactions.GetTransactionById.Response;
using Finance.Application.UseCases.Transactions.GetTransactions.Request;
using Finance.Application.UseCases.Transactions.GetTransactions.Response;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Request;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Response;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary.Request;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary.Response;
using Microsoft.AspNetCore.Mvc;

namespace TransactionServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly IUseCase<CreateTransactionRequest, CreateTransactionResponse> _createTransaction;
        private readonly IUseCase<DeleteTransactionRequest, DeleteTransactionResponse> _deleteTransaction;
        private readonly IUseCase<GetTransactionsByAccountIdRequest, GetTransactionsByAccountIdResponse> _getTransactionByAccountId;
        private readonly IUseCase<GetTransactionByIdRequest, GetTransactionByIdResponse> _getTransactionById;
        private readonly IUseCase<GetTransactionsRequest, GetTransactionsResponse> _getTransactions;
        private readonly IUseCase<GetTransactionsSummaryRequest, GetTransactionSummaryResponse> _getTransactionsSummary;

        public TransactionController(IUseCase<CreateTransactionRequest, CreateTransactionResponse> createTransaction,
                                     IUseCase<DeleteTransactionRequest, DeleteTransactionResponse> deleteTransaction,
                                     IUseCase<GetTransactionsByAccountIdRequest, GetTransactionsByAccountIdResponse> getTransactionByAccountId,
                                     IUseCase<GetTransactionByIdRequest, GetTransactionByIdResponse> getTransactionById,
                                     IUseCase<GetTransactionsRequest, GetTransactionsResponse> getTransactions,
                                     IUseCase<GetTransactionsSummaryRequest, GetTransactionSummaryResponse> getTransactionsSummary)
        {
            _createTransaction = createTransaction;
            _getTransactionByAccountId = getTransactionByAccountId;
            _deleteTransaction = deleteTransaction;
            _getTransactionById = getTransactionById;
            _getTransactions = getTransactions;
            _getTransactionsSummary = getTransactionsSummary;
        }
        [HttpGet("Accounts/{accountId}/Transactions")]
        public async Task<ActionResult<GetTransactionsByAccountIdResponse>> GetTransactionByAccountId(int accountId, CancellationToken ct)
        {
            var request = new GetTransactionsByAccountIdRequest { AccountId = accountId, UserId = 1 };
            var response = await _getTransactionByAccountId.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpGet("{transactionid}")]
        public async Task<ActionResult<GetTransactionByIdResponse>> GetTransactionById(int transactionid, CancellationToken ct)
        {
            var request = new GetTransactionByIdRequest { TransactionId = transactionid };
            var response = await _getTransactionById.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult<GetTransactionsResponse>> GetTransactions(string? type=null, 
                                                                                 int? accountId=null,
                                                                                     string? sortOrder=null, 
                                                                                     string? sortBy=null, 
                                                                                     DateOnly? startDate=null, 
                                                                                     DateOnly? endDate=null, 
                                                                                     CancellationToken ct=default)
        {
            var request = new GetTransactionsRequest(
                            UserId: 1,
                            AccountId:accountId,
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
        public async Task<ActionResult<GetTransactionSummaryResponse>> GetTransactionsSummary(int year,int month,CancellationToken ct)
        {
            var request = new  GetTransactionsSummaryRequest { UserId = 1, Year=year, Month=month };
            var response = await _getTransactionsSummary.ExecuteAsync(request, ct);
            return Ok(response);
        }

        [HttpDelete("{transactionid}")]
        public async Task<ActionResult<DeleteTransactionResponse>> Delete(int transactionid, CancellationToken ct)
        {
            var request = new DeleteTransactionRequest { TransactionId = transactionid };
            var response = await _deleteTransaction.ExecuteAsync(request,ct);
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<CreateTransactionResponse>> Create(CreateTransactionRequest request, CancellationToken ct)
        {
            var response = await _createTransaction.ExecuteAsync(request,ct);
            return Ok(response);
        }
    }
}
