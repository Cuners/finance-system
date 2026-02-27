using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response
{
    public class GetAccountsByUserIdSuccessResponse : GetAccountsByUserIdResponse
    {
        public IEnumerable<AccountDto> Accounts { get; }

        public GetAccountsByUserIdSuccessResponse(IEnumerable<AccountDto> accounts)
        {
            Accounts = accounts;
        }
    }
}
