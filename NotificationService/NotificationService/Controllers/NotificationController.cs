using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTO;
using NotificationService.Domain.Interfaces;
using System.Security.Claims;

namespace NotificationService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _repo;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            INotificationRepository repo,
            ILogger<NotificationsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim ?? throw new UnauthorizedAccessException());
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationDto>>> GetNotifications(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var userId = GetCurrentUserId();
            var notifications = await _repo.GetByUserIdAsync(userId, page, pageSize, ct);

            var dtos = notifications.Select(n => new NotificationDto(
                n.NotificationId,
                n.UserId,
                n.NotificationTypeId,
                n.Title,
                n.Message,
                n.IsRead,
                n.CreatedAt,
                n.EmailSentAt
            )).ToList();

            return Ok(dtos);
        }

        [HttpGet("unread-count")]
        public async Task<ActionResult<int>> GetUnreadCount(CancellationToken ct = default)
        {
            var userId = GetCurrentUserId();
            var count = await _repo.GetUnreadCountAsync(userId, ct);
            return Ok(count);
        }

        [HttpPost("{id}/read")]
        public async Task<ActionResult> MarkAsRead(int id, CancellationToken ct = default)
        {
            var userId = GetCurrentUserId();
            await _repo.MarkAsReadAsync(id, userId, ct);
            return NoContent();
        }

        [HttpPost("mark-all-read")]
        public async Task<ActionResult> MarkAllAsRead(CancellationToken ct = default)
        {
            var userId = GetCurrentUserId();
            await _repo.MarkAllAsReadAsync(userId, ct);
            return NoContent();
        }
    }
}
