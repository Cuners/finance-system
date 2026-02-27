using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.UpdateAccount.Response
{
    public class UpdateAccountSuccessResponse : UpdateAccountResponse
    {
        public int AccountId { get; }

        public UpdateAccountSuccessResponse(int id)
        {
            AccountId = id;
        }
    }
}
