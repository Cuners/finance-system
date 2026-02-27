using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.CreateAccount.Request
{
    public class CreateAccountRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
