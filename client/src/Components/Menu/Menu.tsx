import "./Menu.css"
import MenuItem from "./MenuItem/MenuItem"
import  {useState} from "react"
import baglogo from "/src/assets/IconWhiteBag.png"
import transactlogo from "/src/assets/IconWhiteTransaction.png"
import walletlogo from "/src/assets/IconWhiteWallet.png"
import userlogo from "/src/assets/IconWhiteUser.png"
import infologo from "/src/assets/IconWhiteInfo.png"
import gearlogo from "/src/assets/IconWhiteGear.png"
interface NavItem {
  icon: string;
  label: string;
  to: string;
};

const Menu = () => {
    const [isCollapsed, setIsCollapsed] = useState<boolean>(false);

    const toggleMenu = (): void => {
        setIsCollapsed((prev) => !prev);
    };
const topItems: NavItem[] = [
  { icon: walletlogo, label: "Главная", to: "/main" },
  { icon: transactlogo, label: "Обзор", to: "/expenses" },
  { icon: baglogo, label: "Транзакции", to: "/transactions" },
  { icon: transactlogo, label: "Бюджет", to: "/budget" },
];

const bottomItems: NavItem[] = [
  { icon: userlogo, label: "Авторизация", to: "/auth" },
  { icon: infologo, label: "О приложении", to: "/about" },
  { icon: gearlogo, label: "Настройки", to: "/settings" },
];
    return(
    <nav className={isCollapsed ? "collapsed" : ""}>
        <ul>
            <div className="arrow" onClick={toggleMenu}>

            </div>
            <div className="topDiv">
            {topItems.map((item, index) => (
              <MenuItem
                key={index} 
                icon={item.icon}
                label={item.label}
                to={item.to}
                isCollapsed={isCollapsed}
              />
            ))}
            </div>
            <div className="bottomDiv">
                {bottomItems.map((item, index) => (
                  <MenuItem
                    key={index} 
                    icon={item.icon}
                    label={item.label}
                    to={item.to}
                    isCollapsed={isCollapsed}
                  />
                ))}
            </div>
        </ul>
    </nav>
    );
};

export default Menu;
