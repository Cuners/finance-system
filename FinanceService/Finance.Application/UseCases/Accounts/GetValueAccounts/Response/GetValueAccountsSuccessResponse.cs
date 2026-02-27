using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetValueAccounts.Response
{
    public class GetValueAccountsSuccessResponse : GetValueAccountsResponse
    {
        public decimal Value {  get; }
        public GetValueAccountsSuccessResponse(decimal value)
        {
            Value=value;
        }
    }
}
