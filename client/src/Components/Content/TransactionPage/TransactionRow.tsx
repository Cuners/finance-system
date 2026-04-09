import React, { useState } from 'react';
import { useTransactionDelete } from '../../../Hooks/useTransactionDelete';
import './TransactionRow.css';
import TransactionCreateUpdate from './TransactionCreateUpdate';
import { useTransactionById } from '../../../Hooks/useTransactionById';
interface Transaction {
  id: number;
  title: string;
  category: string;
  date: string;
  status: 'completed' | 'pending' | 'failed';
  amount: number;
  type: 'income' | 'expense';
}

const TransactionRow: React.FC<Transaction> = ({
  id,
  title,
  category,
  date,
  status,
  amount,
  type,
}) => {
  const { deleteTransaction, loading } = useTransactionDelete();
  
  const handleDelete = async () => {
    if (!window.confirm('Are you sure you want to delete this transaction?')) {
      return;
    }
    
    try {
      await deleteTransaction(Number(id));
      window.location.reload(); 
    } catch (error) {
      console.error('Failed to delete transaction:', error);
      alert('Failed to delete transaction. Please try again.');
    }
  };
  
  const formattedAmount = new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);
      const [showEditModal, setShowEditModal] = useState(false); 
    const { data: transactionData, loading: transactionLoading, error } = useTransactionById(showEditModal ? id : null);
    const [showDropdown, setShowDropdown] = useState(false);
    const handleOpenModal = () => {
        setShowEditModal(true); 
        setShowDropdown(false);
    };

    const handleCloseModal = () => {
        setShowEditModal(false);
    };
  return (
    <tr className={`transaction-row ${type}`}>
      <td className="cell icon">
        <div className="icon-bg">
          {type === 'income' ? '💰' : '🛒'}
        </div>
      </td>
      <td className="cell title">{title}</td>
      <td className="cell category">{category}</td>
      <td className="cell date">{date}</td>
      <td className="cell status">
        <span className={`badge ${status}`}>{status}</span>
      </td>
      <td className="cell amount">
       {formattedAmount}
      </td>
      <td className="cell actions">
        <button className="action-btn" onClick={handleOpenModal}>✏️</button>
        <button className="action-btn" onClick={handleDelete} disabled={loading}>
          {loading ? '...' : '🗑️'}
        </button>
      </td>
      <TransactionCreateUpdate
        isOpen={showEditModal && (!transactionLoading ||transactionData)}
        onClose={handleCloseModal}
        mode="edit"
        transaction={transactionData ? {
          transactionId: transactionData.transactionId,
          accountId: transactionData.accountId,
          accountName: transactionData.accountName,
          categoryId: transactionData.categoryId,
          categoryName: transactionData.categoryName,
          amount: transactionData.amount,
          date: transactionData.date,
          note: transactionData.note,
        } : undefined}
        onSuccess={() => {}}
      />
    </tr>
    
  );
};

export default TransactionRow;