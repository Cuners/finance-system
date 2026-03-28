using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.DTO
{
    public record BudgetExceededEvent(int UserId,
                                      string Email,
                                      string AccountName,
                                      decimal Balance,
                                      decimal SpentAmount);
}
