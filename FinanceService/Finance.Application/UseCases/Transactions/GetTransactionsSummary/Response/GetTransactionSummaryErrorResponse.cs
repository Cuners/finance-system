using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary.Response
{
    public class GetTransactionSummaryErrorResponse : GetTransactionSummaryResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetTransactionSummaryErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
