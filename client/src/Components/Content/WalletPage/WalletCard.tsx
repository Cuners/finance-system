import React from 'react';
import DeleteIcon from '../../../assets/SvgIcons/DeleteIcon';
import EditIcon from '../../../assets/SvgIcons/EditIcon';
import HistoryIcon from '../../../assets/SvgIcons/HistoryIcon';
import './WalletCard.css';
import BalanceIcon from '../../../assets/SvgIcons/BalanceIcon';
import { useAccountDelete } from '../../../Hooks/useAccountDelete';
export interface WalletData {
  id: string;
  title: string;
  balance: number;
  accountNumber?: string;
  address?: string;
  description: string;
  isDefault?: boolean;
  transactions: number;
  income: number;
  expenses: number;
  themeColor: string;
}

interface WalletCardProps {
  wallet: WalletData;
  onViewHistory?: (id: string) => void;
  onDelete?: (id: number) => void;
  onEdit?: (wallet: WalletData) => void;
}

const WalletCard: React.FC<WalletCardProps> = ({
  wallet,
  onViewHistory,
  onDelete,
  onEdit
}) => {
  const { deleteAccount, loading,error } = useAccountDelete();
    const handleDelete = async () => {
    try {
      await deleteAccount(parseInt(wallet.id));
      if (onDelete) onDelete(parseInt(wallet.id));
    } catch (error) {
      console.error('Error deleting account:', error);
    }
  };
  return (
    <div className={`wallet-card theme-${wallet.themeColor}`}>
      <div className="wallet-card__header">
        <div className="wallet-card__icon-wrapper">
          <BalanceIcon/>
        </div>
        <div className="wallet-card__title-group">
          <h3 className="wallet-card__title">
            {wallet.title}
          </h3>
        </div>
      </div>

      <div className="wallet-card__balance">
        <span className="wallet-card__currency">RUB</span>
        <div className={`wallet-card__amount ${wallet.balance < 0 ? 'wallet-card__amount--negative' : ''}`}>
          {wallet.balance < 0 ? '-' : ''}${Math.abs(wallet.balance).toLocaleString('ru-RU', { minimumFractionDigits: 2 })}
        </div>
      </div>

      <div className="wallet-card__details">
        <span className="wallet-card__account">
          {wallet.address ? wallet.address : `**** ${wallet.accountNumber}`}
        </span>
        <p className="wallet-card__description">{wallet.description}</p>
      </div>

      <div className="wallet-card__stats">
        <div className="wallet-card__stat">
          <span className="wallet-card__stat-label">Транзакции</span>
          <span className="wallet-card__stat-value">{wallet.transactions}</span>
        </div>
        <div className="wallet-card__stat">
          <span className="wallet-card__stat-label">Доходы</span>
          <span className="wallet-card__stat-value wallet-card__stat-value--income">
            ${wallet.income.toFixed(2)}
          </span>
        </div>
        <div className="wallet-card__stat">
          <span className="wallet-card__stat-label">Расходы</span>
          <span className="wallet-card__stat-value wallet-card__stat-value--expense">
            ${wallet.expenses.toFixed(2)}
          </span>
        </div>
      </div>

      <div className="wallet-card__footer">
        <button 
          className="wallet-card__btn wallet-card__btn--history"
          onClick={() => onViewHistory?.(wallet.id)}>
            <HistoryIcon/> История
        </button>
        <button
          className="wallet-card__btn wallet-card__btn--edit"
          onClick={() => onEdit?.(wallet)}>
            <EditIcon /> Редактировать
        </button>
        <button 
          className="wallet-card__btn wallet-card__btn--delete"
          onClick={handleDelete}>
            <DeleteIcon/>
        </button>
      </div>
    </div>
  );
};

export default WalletCard;