using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Domain
{
    public class NotificationType
    {
        public int NotificationTypeId {  get; set; }
        public string Name { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }=new List<Notification>();  
    }
}
