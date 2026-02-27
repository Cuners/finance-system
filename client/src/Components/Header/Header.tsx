import './Header.css';

const Header = () => {
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
        <button className="notification-btn">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
            <path d="M13.5 3v4"></path>
            <circle cx="18" cy="18" r="3"></circle>
          </svg>
          <span className="badge-header">1</span>
        </button>

        <div className="user-avatar">
          <span>JD</span>
        </div>
      </div>
    </header>
  );
};

export default Header;