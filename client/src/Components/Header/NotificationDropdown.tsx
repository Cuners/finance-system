import React, { useState } from 'react';
import { useNotifications } from '../../Hooks/useNotification';
import './NotificationDropdown.css';

interface NotificationDropdownProps {
  onClose: () => void;
}

export const NotificationDropdown: React.FC<NotificationDropdownProps> = ({ onClose }) => {
  const { 
    notifications, 
    unreadCount, 
    isLoading, 
    error,
    markAsRead, 
    markAllAsRead,
    refresh 
  } = useNotifications();

  const [isRefreshing, setIsRefreshing] = useState(false);

  const handleRefresh = async () => {
    setIsRefreshing(true);
    await refresh();
    setIsRefreshing(false);
  };

  const formatTime = (isoString: string) => {
    const date = new Date(isoString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Только что';
    if (diffMins < 60) return `${diffMins} мин. назад`;
    if (diffHours < 24) return `${diffHours} ч. назад`;
    if (diffDays < 7) return `${diffDays} дн. назад`;
    
    return date.toLocaleDateString('ru-RU', {
      day: '2-digit',
      month: '2-digit'
    });
  };

  const getIcon = (type: string) => {
    switch (type) {
      case 'transaction': return '💰';
      case 'budget': return '📊';
      case 'auth': return '🔐';
      default: return '🔔';
    }
  };

  const getColor = (type: string) => {
    switch (type) {
      case 'transaction': return '#10b981';
      case 'budget': return '#ef4444';
      case 'auth': return '#3b82f6';
      default: return '#6b7280';
    }
  };

  return (
    <div className="notification-dropdown">
      {/* Header */}
      <div className="notification-header">
        <h3>Уведомления</h3>
        <div className="notification-actions">
          {unreadCount > 0 && (
            <button 
              className="btn-mark-all"
              onClick={markAllAsRead}
              disabled={isLoading}
            >
              Прочитать все
            </button>
          )}
          <button 
            className="btn-refresh"
            onClick={handleRefresh}
            disabled={isRefreshing || isLoading}
            title="Обновить"
          >
            {isRefreshing ? '⟳' : '↻'}
          </button>
          <button className="btn-close" onClick={onClose}>✕</button>
        </div>
      </div>

      {/* Content */}
      <div className="notification-content">
        {isLoading && notifications.length === 0 ? (
          <div className="notification-loading">
            <div className="spinner"></div>
            <p>Загрузка...</p>
          </div>
        ) : error ? (
          <div className="notification-error">
            <p>⚠️ {error}</p>
            <button onClick={refresh}>Повторить</button>
          </div>
        ) : notifications.length === 0 ? (
          <div className="notification-empty">
            <p>🎉 Нет новых уведомлений</p>
          </div>
        ) : (
          <ul className="notification-list">
            {notifications.map(notification => (
              <li 
                key={notification.notificationId}
                className={`notification-item ${!notification.isRead ? 'unread' : ''}`}
                onClick={() => !notification.isRead && markAsRead(notification.notificationId)}
              >
                <div className="notification-icon" style={{ color: getColor(notification.type) }}>
                  {getIcon(notification.type)}
                </div>
                <div className="notification-body">
                  <div className="notification-title">{notification.title}</div>
                  <div className="notification-message">{notification.message}</div>
                  <div className="notification-time">{formatTime(notification.createdAt)}</div>
                </div>
                {!notification.isRead && (
                  <div className="notification-badge"></div>
                )}
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Footer */}
      <div className="notification-footer">
        <a href="/notifications" onClick={(e) => { e.preventDefault(); onClose(); }}>
          Все уведомления →
        </a>
      </div>
    </div>
  );
};