using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.UpdateAccount.Request
{
    public class UpdateAccountRequest
    {
        public int AccountId {  get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
