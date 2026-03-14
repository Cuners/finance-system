import React, { useState} from 'react';
import WalletCard, { type WalletData } from './WalletCard';
import WalletHistoryModal from './WalletHistoryModal';
import WalletCreateUpdate from './WalletCreateUpdate';
import './WalletPage.css';
import { useRecentAccounts } from '../../../Hooks/useAccounts';
import { useAccountById } from '../../../Hooks/useAccountById';
import { type AccountSummaryDto, type AccountDto } from '../../../Types';
import TransactionsHeader from "../ContentHeader.tsx";
const mapAccountToWalletData = (account: AccountSummaryDto): WalletData => {
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
  const [isHistoryModalOpen, setIsHistoryModalOpen] = useState(false);
  const [selectedWalletId, setSelectedWalletId] = useState<number | null>(null);
  const [isCreateUpdateModalOpen, setIsCreateUpdateModalOpen] = useState(false);
  const [editingWallet, setEditingWallet] = useState<AccountSummaryDto | AccountDto | null>(null);
  const [editWalletId, setEditWalletId] = useState<number | null>(null);
  
  const { data: walletToEdit, loading: loadingWalletToEdit } = useAccountById(editWalletId);

  // Effect to handle opening the edit modal when wallet data is loaded
  React.useEffect(() => {
    if (walletToEdit && editWalletId !== null) {
      handleOpenCreateUpdateModal(walletToEdit);
      setEditWalletId(null); // Reset the ID to prevent reopening
    }
  }, [walletToEdit, editWalletId]);

  const wallets = accounts.map(mapAccountToWalletData);

  const handleViewHistory = (id: string) => {
    const wallet = wallets.find(w => w.id === id);
    if (wallet) {
      setSelectedWallet(wallet);
      setSelectedWalletId(parseInt(id));
      setIsHistoryModalOpen(true);
    }
  };

  const handleOpenCreateUpdateModal = (wallet: AccountSummaryDto | AccountDto | null = null) => {
    setEditingWallet(wallet as AccountSummaryDto | null);
    setIsCreateUpdateModalOpen(true);
  };

  const handleCloseCreateUpdateModal = () => {
    setIsCreateUpdateModalOpen(false);
    setEditingWallet(null);
  };

  const handleSuccess = () => {
    // Refresh the accounts by triggering a reload
    window.location.reload();
  };

  const handleCloseHistoryModal = () => {
    setIsHistoryModalOpen(false);
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
        <div className="header-controls">
          <TransactionsHeader
            title="Кошельки"
            description="Просматривайте ваши кошельки"/>
          <button className="add-btn" onClick={() => handleOpenCreateUpdateModal()}>+ Добавить кошелёк</button>
        </div>
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
              onEdit={(walletData) => {
                const id = parseInt(walletData.id);
                setEditWalletId(id);
              }}
            />
          ))}
        </div>
      </div>

      {selectedWallet && selectedWalletId !== null && (
        <WalletHistoryModal
          isOpen={isHistoryModalOpen}
          onClose={handleCloseHistoryModal}
          walletId={selectedWalletId}
          walletTitle={selectedWallet.title}
        />
      )}
      <WalletCreateUpdate
        isOpen={isCreateUpdateModalOpen}
        onClose={handleCloseCreateUpdateModal}
        wallet={editingWallet || null}
        onSuccess={handleSuccess}
      />
    </>
  );
};

export default WalletDashboard;