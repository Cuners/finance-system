using Finance.Application.UseCases.Accounts.GetAccountById.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountById.Response
{
    public class GetAccountByIdSuccessResponse : GetAccountByIdResponse
    {
        public AccountDto Account { get; }

        public GetAccountByIdSuccessResponse(AccountDto accountDto)
        {
            Account = accountDto;
        }
    }
}
