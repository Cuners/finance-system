using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.DTO.Events
{
    public record UserCreatedEvent(int UserId,
                                   string Email,
                                   string Login
    );
}
