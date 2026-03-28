using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.DTO
{
    public record NotificationDto(int Id,
                                  int UserId,
                                  int TypeId,
                                  string Title,
                                  string Message,
                                  bool IsRead,
                                  DateTime CreatedAt,
                                  DateTime? EmailSentAt
    );
}
