import React, { useEffect } from 'react';
import TransactionItem from '../MainPage/TransactionItem';
import './WalletHistory.css';

export interface Transaction {
  id: string;
  title: string;
  category: string;
  date: string;
  amount: number;
  status: 'completed' | 'pending' | 'failed';
  type: 'income' | 'expense';
  icon?: React.ReactNode;
}

interface WalletHistoryProps {
  isOpen: boolean;
  onClose: () => void;
  walletTitle: string;
  transactions: Transaction[];
}

const getIconForCategory = (category: string, type: 'income' | 'expense'): React.ReactNode => {
  const categoryLower = category.toLowerCase();
  
  if (categoryLower.includes('income') || categoryLower.includes('salary') || categoryLower.includes('bonus')) {
    return '💼';
  }
  if (categoryLower.includes('food') || categoryLower.includes('grocery')) {
    return '🛒';
  }
  if (categoryLower.includes('electric') || categoryLower.includes('utilities')) {
    return '⚡';
  }
  if (categoryLower.includes('health') || categoryLower.includes('gym')) {
    return '❤️';
  }
  if (categoryLower.includes('restaurant') || categoryLower.includes('dinner')) {
    return '🍽️';
  }
  if (categoryLower.includes('investment') || categoryLower.includes('stock')) {
    return '📈';
  }
  if (categoryLower.includes('crypto') || categoryLower.includes('bitcoin')) {
    return '₿';
  }
  
  return type === 'income' ? '💰' : '💸';
};

const WalletHistory: React.FC<WalletHistoryProps> = ({
  isOpen,
  onClose,
  walletTitle,
  transactions
}) => {
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };

    if (isOpen) {
      document.addEventListener('keydown', handleEscape);
      document.body.style.overflow = 'hidden';
    }

    return () => {
      document.removeEventListener('keydown', handleEscape);
      document.body.style.overflow = 'unset';
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  const totalIncome = transactions
    .filter(t => t.type === 'income')
    .reduce((sum, t) => sum + t.amount, 0);

  const totalExpenses = transactions
    .filter(t => t.type === 'expense')
    .reduce((sum, t) => sum + Math.abs(t.amount), 0);

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title-wrapper">
            <h2 className="modal-title">{walletTitle}</h2>
            <span className="modal-subtitle">История транзакций</span>
          </div>
          <button className="modal-close" onClick={onClose}>
            &times;
          </button>
        </div>

        <div className="modal-body">
          <div className="transactions-list">
            {transactions.length === 0 ? (
              <div className="transactions-empty">
                <p>No transactions found</p>
              </div>
            ) : (
              transactions.map((transaction) => (
                <TransactionItem
                  key={transaction.id}
                  title={transaction.title}
                  category={transaction.category}
                  type={transaction.type}
                  amount={transaction.type === 'income' ? transaction.amount : -Math.abs(transaction.amount)}
                  date={transaction.date}
                  icon={transaction.icon || getIconForCategory(transaction.category, transaction.type)}
                />
              ))
            )}
          </div>
        </div>

        <div className="modal-footer">
          <div className="footer-stat">
            <span className="footer-stat-label">Количество транзакций</span>
            <span className="footer-stat-value">{transactions.length}</span>
          </div>
          <div className="footer-stat">
            <span className="footer-stat-label">Доходы</span>
            <span className="footer-stat-value footer-stat-income">
              {new Intl.NumberFormat('ru-RU', {
                style: 'currency',
                currency: 'RUB',
                minimumFractionDigits: 2,
              }).format(totalIncome)}
            </span>
          </div>
          <div className="footer-stat">
            <span className="footer-stat-label">Расходы</span>
            <span className="footer-stat-value footer-stat-expense">
              {new Intl.NumberFormat('ru-RU', {
                style: 'currency',
                currency: 'RUB',
                minimumFractionDigits: 2,
              }).format(totalExpenses)}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default WalletHistory;