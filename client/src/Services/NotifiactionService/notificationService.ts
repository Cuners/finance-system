import type { Notification, NotificationType, NotificationDto } from '../../Types/index';
import { httpService } from '../httpService';

const API_BASE = '/api/notifications';

export const notificationService = {
  getNotifications(page: number = 1, pageSize: number = 20): Promise<NotificationDto[]> {
    return httpService.get<NotificationDto[]>(
      `${API_BASE}?page=${page}&pageSize=${pageSize}`
    );
  },
  getUnreadCount(): Promise<number> {
    return httpService.get<number>(`${API_BASE}/unread-count`);
  },
  markAsRead(id: number): Promise<void> {
    return httpService.post<void>(`${API_BASE}/${id}/read`, null);
  },
  markAllAsRead(): Promise<void> {
    return httpService.post<void>(`${API_BASE}/mark-all-read`, null);
  },
  getNotificationTypes(): Promise<NotificationType[]> {
    return httpService.get<NotificationType[]>(`${API_BASE}/types`);
  },
};