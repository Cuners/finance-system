namespace NotificationService.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? EmailSentAt { get; set; }
        public string? Data { get; set; }
        public virtual NotificationType NotificationType { get; set; } = null!;
    }
}
