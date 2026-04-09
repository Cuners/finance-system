import React, { useState, useEffect } from 'react';
import { useTransactionAddUpdate } from '../../../Hooks/useTransactionAddUpdate';
import { useCategories } from '../../../Hooks/useCategories';
import { useRecentAccounts } from '../../../Hooks/useAccounts';
import type { AccountDto } from '../../../Types';
import './TransactionCreateUpdate.css';

interface TransactionFormProps {
  isOpen: boolean;
  onClose: () => void;
  mode: 'create' | 'edit';
  transaction?: {
    transactionId: number;
    accountId: number;
    accountName: string;
    categoryId: number;
    categoryName: string;
    amount: number;
    date: Date;
    note: string;
  } | null;
  onSuccess?: () => void;
}

const TransactionCreateUpdate: React.FC<TransactionFormProps> = ({
  isOpen,
  onClose,
  mode,
  transaction,
  onSuccess
}) => {
  const { createTransaction, updateTransaction, loading, error } = useTransactionAddUpdate();
  const { categories, loading: categoriesLoading } = useCategories();
  const { accounts, loading: accountsLoading } = useRecentAccounts();

  const [formData, setFormData] = useState({
    accountId: 0,
    accountName: '',
    categoryId: 0,
    categoryName: '',
    amount: 0,
    date: new Date().toISOString().split('T')[0],
    note: ''
  });

  useEffect(() => {
    if (mode==='edit' && transaction) {
      setFormData({
        accountId: transaction.accountId,
        accountName: transaction.accountName,
        categoryId: transaction.categoryId,
        categoryName: transaction.categoryName,
        amount: transaction.amount,
        date: new Date(transaction.date).toISOString().split('T')[0],
        note: transaction.note || ''
      });
    } else {
      setFormData({
        accountId: 0,
        accountName: '',
        categoryId: 0,
        categoryName: '',
        amount: 0,
        date: new Date().toISOString().split('T')[0],
        note: ''
      });
    }
  }, [mode, transaction]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'amount' ? parseFloat(value) || 0 : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      const transactionData = {
        ...formData,
        date: new Date(formData.date),
      };
      
      if (mode==='edit' && transaction) {
        await updateTransaction({
          ...transaction,
          ...transactionData,
          transactionId: transaction.transactionId
        });
      } else {
        await createTransaction(transactionData);
      }
      
      if (onSuccess) {
        onSuccess();
      }
      onClose();
    } catch (err) {
      console.error('Error saving transaction:', err);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2 className="modal-title">{mode==='edit' ? 'Редактировать транзакцию' : 'Создать транзакцию'}</h2>
          <button className="close-button" onClick={onClose}>&times;</button>
        </div>
        
        {error && <div className="error-message">{error}</div>}
        
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="accountId" className="form-label">Account</label>
              <select
                id="accountId"
                name="accountId"
                value={formData.accountId}
                onChange={handleChange}
                className="form-select"
                required
              >
                <option value="">Select Account</option>
                {accounts.map((account: AccountDto) => (
                  <option key={account.accountId} value={account.accountId}>
                    {account.name}
                  </option>
                ))}
              </select>
            </div>
            
            <div className="form-group">
              <label htmlFor="categoryId" className="form-label">Category</label>
              <select
                id="categoryId"
                name="categoryId"
                value={formData.categoryId}
                onChange={handleChange}
                className="form-select"
                required
              >
                <option value="">Select Category</option>
                {categories.map(category => (
                  <option key={category.categoryId} value={category.categoryId}>
                    {category.name}
                  </option>
                ))}
              </select>
            </div>
          </div>
          
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="amount" className="form-label">Amount</label>
              <input
                type="number"
                id="amount"
                name="amount"
                value={formData.amount}
                onChange={handleChange}
                className="form-input"
                min="0"
                step="0.01"
                required
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="date" className="form-label">Date</label>
              <input
                type="date"
                id="date"
                name="date"
                value={formData.date}
                onChange={handleChange}
                className="form-input"
                required
              />
            </div>
          </div>
          
          <div className="form-group">
            <label htmlFor="note" className="form-label">Note</label>
            <textarea
              id="note"
              name="note"
              value={formData.note}
              onChange={handleChange}
              className="form-input"
              rows={3}
              placeholder="Enter transaction note"
            />
          </div>
          
          <div className="button-group">
            <button
              type="button"
              className="btn btn-secondary"
              onClick={onClose}
              disabled={loading}
            >
              Cancel
            </button>
            
            <button
              type="submit"
              className="btn btn-primary"
              disabled={loading || categoriesLoading || accountsLoading}
            >
              {loading ? <span className="loading-spinner"></span> : ''}
              {mode==='edit' ? 'Редактировать' : 'Создать'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TransactionCreateUpdate;