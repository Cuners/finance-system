
import { Routes,Route } from 'react-router'
import "./App.css"
import Menu from "./Components/Menu/Menu"
import AuthorizationPage from './Components/Content/AuthorizationPage/AuthorizationPage';
import WalletPage from './Components/Content/WalletPage/WalletPage';
import BudgetPage from "./Components/Content/BudgetPage/BudgetPage";
import InfoPage from './Components/Content/InfoPage/InfoPage';
import SettingsPage from "./Components/Content/SettingsPage/SettingsPage";
import RegistrationPage from './Components/Content/AuthorizationPage/RegistrationPage';
import Header from './Components/Header/Header';
import TransactionPage from './Components/Content/TransactionPage/TransactionPage';
import MainPage from './Components/Content/MainPage/MainPage';
function App() {
  return (
    <>
    <div className="app-layout">
        <Menu></Menu>
        <div className="main-content">
          <Header></Header>
            <div className="content-area">
              <Routes>
                <Route path="/" element={<WalletPage />} />
                <Route path="/auth" element={<AuthorizationPage />} />
                <Route path="/wallets" element={<WalletPage />} />
                <Route path="/budget" element={<BudgetPage />} />
                <Route path="/transactions" element={<TransactionPage/>} />
                <Route path="/main" element={<MainPage/>} />
                <Route path="/about" element={<InfoPage />} />
                <Route path="/settings" element={<SettingsPage />} />
                <Route path="/register" element={<RegistrationPage/>} />
              </Routes>
            </div>
        </div>
      </div>
    </>
  );
};

export default App;
