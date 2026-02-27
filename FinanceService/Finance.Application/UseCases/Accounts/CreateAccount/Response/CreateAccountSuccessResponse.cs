using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.CreateAccount.Response
{
    public class CreateAccountSuccessResponse : CreateAccountResponse
    {
        public int AccountId { get; }

        public CreateAccountSuccessResponse(int id)
        {
            AccountId = id;
        }
    }
}
