using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Request
{
    public class GetTransactionsByAccountIdRequest
    {
        public int AccountId {  get; set; }
    }
}
