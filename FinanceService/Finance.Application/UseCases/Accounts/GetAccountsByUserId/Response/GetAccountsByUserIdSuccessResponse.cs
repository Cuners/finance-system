using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response
{
    public class GetAccountsByUserIdSuccessResponse : GetAccountsByUserIdResponse
    {
        public IEnumerable<AccountSummaryDto> Accounts { get; }

        public GetAccountsByUserIdSuccessResponse(IEnumerable<AccountSummaryDto> accounts)
        {
            Accounts = accounts;
        }
    }
}
