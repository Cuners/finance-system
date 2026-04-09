import { useState, useEffect, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import type { Notification, NotificationData, UseNotificationsReturn } from '../Types/index';
import { httpService } from '../Services/httpService';
import { createSignalRConnection } from '../Services/signalr';

const API_BASE = import.meta.env.VITE_API_URL || 'http://localhost:5002';

export const useNotifications = (): UseNotificationsReturn => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [isConnected, setIsConnected] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  const loadNotifications = useCallback(async () => {
    try {
      setIsLoading(true);
      const response = await httpService.get<Notification[]>(
        `${API_BASE}/api/notifications?page=1&pageSize=20`
      );
      
      setNotifications(response);
      setError(null);
    } catch (err) {
      console.error('Failed to load notifications:', err);
      setError('Не удалось загрузить уведомления');
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Загрузка количества непрочитанных
  const loadUnreadCount = useCallback(async () => {
    try {
      const count = await httpService.get<number>(
        `${API_BASE}/api/notifications/unread-count`
      );
      setUnreadCount(count);
    } catch (err) {
      console.error('Failed to load unread count:', err);
    }
  }, []);

  // Отметить как прочитанное
  const markAsRead = useCallback(async (id: number) => {
    try {
      await httpService.post<void>(
        `${API_BASE}/api/notifications/${id}/read`,
        null
      );
      setNotifications(prev => 
        prev.map(n => n.notificationId === id ? { ...n, isRead: true } : n)
      );
      setUnreadCount(prev => Math.max(0, prev - 1));
    } catch (err) {
      console.error('Failed to mark as read:', err);
    }
  }, []);

  // Отметить все как прочитанные
  const markAllAsRead = useCallback(async () => {
    try {
      await httpService.post<void>(`${API_BASE}/api/notifications/mark-all-read`,null);
      
      setNotifications(prev => prev.map(n => ({ ...n, isRead: true })));
      setUnreadCount(0);
    } catch (err) {
      console.error('Failed to mark all as read:', err);
    }
  }, []);

  // Инициализация SignalR
  useEffect(() => {
    const connection = createSignalRConnection();
    connectionRef.current = connection;
    connection.on('ReceiveNotification', (data: NotificationData) => {
      console.log('🔔 New notification:', data);
      if ('Notification' in window && Notification.permission === 'granted') {
        new Notification('Budget App', {
          body: (data.data as any)?.message || 'Новое уведомление',
          icon: '/favicon.ico'
        });
      }
      setUnreadCount(prev => prev + 1);
    });
    connection.start()
      .then(() => {
        console.log('SignalR connected');
        setIsConnected(true);
        return connection.invoke('Subscribe');
      })
      .then(() => {
        console.log('Subscribed to notifications');
        loadNotifications();
        loadUnreadCount();
      })
      .catch(err => {
        console.error('SignalR connection error:', err);
        setError('Не удалось подключиться к уведомлениям');
        setIsConnected(false);
      });
    return () => {
      connection.stop().catch(console.error);
    };
  }, [loadNotifications, loadUnreadCount]);

  // Перезагрузка
  const refresh = useCallback(() => {
    loadNotifications();
    loadUnreadCount();
  }, [loadNotifications, loadUnreadCount]);

  return {
    notifications,
    unreadCount,
    isConnected,
    isLoading,
    error,
    loadNotifications,
    loadUnreadCount,
    markAsRead,
    markAllAsRead,
    refresh
  };
};