using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.DTO.Events
{
    public record TransactionCreatedEvent(int UserId,
                                          string Email,
                                          string AccountName,
                                          decimal Balance,
                                          decimal SpentAmount
    );  
}
