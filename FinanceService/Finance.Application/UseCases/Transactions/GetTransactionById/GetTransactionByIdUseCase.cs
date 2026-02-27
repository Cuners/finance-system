using Finance.Application.UseCases.Transactions;
using Finance.Application.UseCases.Transactions.GetTransactionById;
using Finance.Application.UseCases.Transactions.GetTransactionById.Request;
using Finance.Application.UseCases.Transactions.GetTransactionById.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionById
{
    public class GetTransactionByIdUseCase
    {
        private readonly ITransactionRepository _TransactionRepository;
        private readonly ILogger<GetTransactionByIdUseCase> _logger;
        public GetTransactionByIdUseCase(ITransactionRepository TransactionRepository, ILogger<GetTransactionByIdUseCase> logger)
        {
            _TransactionRepository = TransactionRepository;
            _logger = logger;
        }
        public async Task<GetTransactionByIdResponse> ExecuteAsync(GetTransactionByIdRequest request, CancellationToken ct)
        {
            try
            {
                if (request.TransactionId <= 0)
                {
                    _logger.LogWarning("GetTransactionRequest is null");
                    return new GetTransactionByIdErrorResponse("Invalid transactions id", "INVALID_USER_ID");
                }
                var transactions = await _TransactionRepository.GetTransactionByTransactionId(request.TransactionId,ct);
                if (transactions == null)
                {
                    _logger.LogWarning("GetTransactionRequest is null");
                    return new GetTransactionByIdErrorResponse("No transactions found", "Transaction_NOT_FOUND");
                }
                var result= new TransactionDto(transactions.TransactionId,
                    transactions.AccountId, 
                    transactions.Account.Name, 
                    transactions.CategoryId, 
                    transactions.Category.Name, 
                    transactions.Amount, 
                    transactions.Date, 
                    transactions.Note);
                return new GetTransactionByIdSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetTransactionByIdErrorResponse("Unable to get transactions at this time", "INVALID_GET");
            }
        }
    }
}
