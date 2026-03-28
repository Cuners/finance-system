import { useState, useRef, useEffect } from 'react';
import { NotificationDropdown } from './NotificationDropdown';
import './Header.css';

const Header = () => {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [unreadCount, setUnreadCount] = useState(0);
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Простая логика обновления счётчика (в реальном проекте используйте useNotifications)
  const updateUnreadCount = () => {
    // Здесь можно вызвать API или использовать контекст
    setUnreadCount(1); // Пример
  };

  useEffect(() => {
    updateUnreadCount();
  }, []);

  // Закрытие dropdown при клике вне
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsDropdownOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const toggleDropdown = () => {
    setIsDropdownOpen(prev => !prev);
    if (!isDropdownOpen) {
      updateUnreadCount();
    }
  };

  return (
    <header className="header">
      <div className="header-left">
        <div className="search-box">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <circle cx="11" cy="11" r="8"></circle>
            <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
          </svg>
          <input type="text" placeholder="Поиск..." />
        </div>
      </div>

      <div className="header-right">
        {/* Notification Button */}
        <div className="notification-wrapper" ref={dropdownRef}>
          <button 
            className="notification-btn" 
            onClick={toggleDropdown}
            aria-label="Уведомления"
          >
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor">
              <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
              <path d="M13.5 3v4"></path>
              <circle cx="18" cy="18" r="3"></circle>
            </svg>
            {unreadCount > 0 && (
              <span className="badge-header">
                {unreadCount > 99 ? '99+' : unreadCount}
              </span>
            )}
          </button>

          {isDropdownOpen && (
            <NotificationDropdown onClose={() => setIsDropdownOpen(false)} />
          )}
        </div>

        <div className="user-avatar">
          <span>JD</span>
        </div>
      </div>
    </header>
  );
};

export default Header;