using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.DeleteAccount.Response
{
    public class DeleteAccountSuccessResponse : DeleteAccountResponse
    {
        public int AccountId { get; }

        public DeleteAccountSuccessResponse(int id)
        {
            AccountId = id;
        }
    }
}
