using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.DTO
{
    public record TransactionDto
         (
            int TransactionId,
            int AccountId,
            string? AccountName,
            int CategoryId ,
            string? CategoryName,
            decimal Amount,
            DateOnly Date,
            string? Note
        );
}
    