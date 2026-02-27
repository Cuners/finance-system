
import { Link, useLocation } from 'react-router';

interface MenuItemProps {
  icon: string;
  label: string;
  to: string;
  isCollapsed: boolean;
}

const MenuItem = ({ icon, label, to, isCollapsed }: MenuItemProps) => {
  const location = useLocation();
  const isActive = location.pathname === to;

  return (
    <li>
      <Link to={to} className={isActive ? 'active' : ''}>
        <img className="logoImg" src={icon} alt={label} />
        {!isCollapsed && <span>{label}</span>}
      </Link>
    </li>
  );
};

export default MenuItem;