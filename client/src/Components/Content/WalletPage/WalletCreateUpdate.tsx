import React, { useState, useEffect } from 'react';
import type { AccountDto } from '../../../Types';
import { useWalletAddUpdate } from '../../../Hooks/useWalletAddUpdate';
import './WalletCreateUpdate.css';

interface WalletCreateUpdateProps {
  isOpen: boolean;
  onClose: () => void;
  wallet?: AccountDto | null;
  onSuccess: () => void;
}

const WalletCreateUpdate: React.FC<WalletCreateUpdateProps> = ({ 
  isOpen, 
  onClose, 
  wallet = null, 
  onSuccess 
}) => {
  const isEditing = !!wallet;
  const { createWallet, updateWallet, loading } = useWalletAddUpdate();
  const [formData, setFormData] = useState<Omit<AccountDto, 'accountId'> & { accountId?: number }>({
    name: '',
    balance: 0,
    note: ''
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (isOpen) {
      if (isEditing && wallet) {
        setFormData({
          accountId: wallet.accountId,
          name: wallet.name || '',
          balance: wallet.balance || 0,
          note: wallet.note || ''
        });
      } else {
        setFormData({
          name: '',
          balance: 0,
          note: ''
        });
        setErrors({});
      }
    }
  }, [isOpen, wallet, isEditing]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'balance' ? Number(value) : value
    }));
    
    if (errors[name]) {
      setErrors(prev => {
        const newErrors = { ...prev };
        delete newErrors[name];
        return newErrors;
      });
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.name.trim()) {
      newErrors.name = 'Название кошелька необходимо';
    }

    if (formData.balance < 0) {
      newErrors.balance = 'Баланс не может быть негативным';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      if (isEditing) {
        await updateWallet({
          accountId: formData.accountId!,
          name: formData.name,
          balance: formData.balance,
          note: formData.note
        });
      } else {
        await createWallet({
          name: formData.name,
          balance: formData.balance,
          note: formData.note
        });
      }
      
      onSuccess();
      onClose();
    } catch (error) {
      console.error('Error saving wallet:', error);
      setErrors({ submit: 'Failed to save wallet. Please try again.' });
    }
  };

  if (!isOpen) {
    return null;
  }

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="wallet-modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="wallet-modal-header">
          <h2>{isEditing ? 'Edit Wallet' : 'Create Wallet'}</h2>
          <button className="wallet-close-button" onClick={onClose}>×</button>
        </div>
        
        <form onSubmit={handleSubmit} className="wallet-form">
          <div className="form-group">
            <label htmlFor="name">Название кошелька</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              className={errors.name ? 'error' : ''}
              placeholder="Введите название кошелька"
            />
            {errors.name && <span className="error-message">{errors.name}</span>}
          </div>
          
          <div className="form-group">
            <label htmlFor="balance">Баланс</label>
            <input
              type="number"
              id="balance"
              name="balance"
              value={formData.balance}
              onChange={handleChange}
              min="0"
              step="1"
              className={errors.balance ? 'error' : ''}
              placeholder="0.00"
            />
            {errors.balance && <span className="error-message">{errors.balance}</span>}
          </div>
          
          <div className="form-group">
            <label htmlFor="note">Заметка</label>
            <textarea
              id="note"
              name="note"
              value={formData.note}
              onChange={handleChange}
              placeholder="Введите заметку (опционально)"
              rows={3}
            />
          </div>
          
          {errors.submit && <div className="error-message submit-error">{errors.submit}</div>}
          
          <div className="form-actions">
            <button type="button" className="cancel-button" onClick={onClose}>
              Отмена
            </button>
            <button type="submit" className="save-button" disabled={loading}>
              {loading ? 'Сохранение...' : (isEditing ? 'Редактировать кошеллек' : 'Сохранить кошеллек')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default WalletCreateUpdate;