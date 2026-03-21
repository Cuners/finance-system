using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.DTO.Events
{
    public record TransactionCreatedEvent(int UserId,
                                          int TransactionId,
                                          decimal Amount,
                                          string Category,
                                          DateTime Date
    );  
}
