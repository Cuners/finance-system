import React, { useState} from 'react';
import WalletCard, { type WalletData } from './WalletCard';
import WalletHistoryModal from './WalletHistoryModal';
import './WalletPage.css';
import { useRecentAccounts } from '../../../Hooks/useAccounts';
import { type AccountDto} from '../../../Types';

const mapAccountToWalletData = (account: AccountDto): WalletData => {
  return {
    id: account.accountId.toString(),
    title: account.name,
    balance: account.balance,
    accountNumber: account.accountId.toString().slice(-4).padStart(4, '0'),
    description: account.note || '',
    transactions: account.count,
    income: account.income,
    expenses: account.expense,
    themeColor: getThemeColor(account.accountId),
  };
};
const getThemeColor = (id: number): string => {
  const colors = ['teal', 'blue', 'green', 'purple', 'orange', 'yellow'];
  return colors[id % colors.length];
};

const WalletDashboard: React.FC = () => {
  const { accounts, loading, error } = useRecentAccounts();
  const [selectedWallet, setSelectedWallet] = useState<WalletData | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedWalletId, setSelectedWalletId] = useState<number | null>(null);

  const wallets = accounts.map(mapAccountToWalletData);

  const handleViewHistory = (id: string) => {
    const wallet = wallets.find(w => w.id === id);
    if (wallet) {
      setSelectedWallet(wallet);
      setSelectedWalletId(parseInt(id));
      setIsModalOpen(true);
    }
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setTimeout(() => {
      setSelectedWallet(null);
      setSelectedWalletId(null);
    }, 300);
  };

  if (loading) {
    return <div>Loading accounts...</div>;
  }

  if (error) {
    return <div>Error loading accounts: {error}</div>;
  }

  return (
    <>
      <div className="dashboard-container">
        <header className="dashboard-header">
          <div className="search-bar-wrapper">
            <input type="text" placeholder="Search wallets..." className="search-input" />
          </div>
          <div className="filter-dropdown">
            <span>Все типы</span>
          </div>
        </header>

        <div className="wallets-grid">
          {wallets.map((wallet) => (
            <WalletCard
              key={wallet.id}
              wallet={wallet}
              onViewHistory={handleViewHistory}
            />
          ))}
        </div>
      </div>

      {selectedWallet && selectedWalletId !== null && (
        <WalletHistoryModal
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          walletId={selectedWalletId}
          walletTitle={selectedWallet.title}
        />
      )}
    </>
  );
};

export default WalletDashboard;