using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts
{
    public class AccountDto
    {
        public int AccountId { get; init; }
        public string Name { get; init; } = "";
        public decimal Balance { get; init; }
    }
}
