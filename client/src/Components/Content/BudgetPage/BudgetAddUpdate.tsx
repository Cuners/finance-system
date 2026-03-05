// AddBudgetModal.tsx
import React, { useState, useEffect } from 'react';
import { useCategories } from '../../../Hooks/useCategories';
import { useBudgetAddUpdate } from '../../../Hooks/useBudgetAddUpdate';
import { getCategoryIcon, getCategoryColor } from '../../../Utils/CategoryIcons';
import './BudgetAddUpdate.css';

interface AddBudgetModalProps {
  isOpen: boolean;
  onClose: () => void;
  mode: 'create' | 'edit';
  budgetData?: {
    budgetId: number;
    categoryId: number;
    categoryName: string;
    amount: number;
  };
  onSuccess?: () => void;
}

const AddBudgetModal = ({
  isOpen,
  onClose,
  mode,
  budgetData,
  onSuccess
}: AddBudgetModalProps) => {
  const [amount, setAmount] = useState('');
  const [selectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { categories, loading, error} = useCategories();
  const { createBudget, updateBudget, loading: budgetMutationLoading } = useBudgetAddUpdate();

  useEffect(() => {
    if (mode === 'edit' && budgetData) {
      setAmount(budgetData.amount.toString());
      setSelectedCategoryId(budgetData.categoryId);
    } else if (mode === 'create') {
      setAmount('');
      setSelectedCategoryId(null);
      setErrors({});
    }
  }, [mode, budgetData]);

  const validateForm = () => {
    const newErrors: { [key: string]: string } = {};
    
    const parsedAmount = parseFloat(amount);
    if (!amount || isNaN(parsedAmount) || parsedAmount <= 0) {
      newErrors.amount = 'Введите корректную сумму';
    }
    
    if (!selectedCategoryId) {
      newErrors.category = 'Выберите категорию';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    setIsSubmitting(true);
    
    try {
      if (mode === 'create') {
        await createBudget({
          categoryId: selectedCategoryId!,
          categoryName: categories.find((cat: any) => cat.categoryId === selectedCategoryId)?.name || '',
          amount: parseFloat(amount)
        });
      } else if (mode === 'edit' && budgetData) {
        await updateBudget({
          budgetId: budgetData.budgetId,
          categoryId: selectedCategoryId!,
          categoryName: categories.find((cat: any) => cat.categoryId === selectedCategoryId)?.name || '',
          amount: parseFloat(amount)
        });
      }
      
      setAmount('');
      setSelectedCategoryId(null);
      setErrors({});
      
      if (onSuccess) {
        onSuccess(); 
      }
      
      onClose(); 
    } catch (error) {
      console.error('Error saving budget:', error);
      setErrors({ submit: 'Ошибка при сохранении бюджета' });
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{mode === 'create' ? 'Добавить бюджет' : 'Редактировать бюджет'}</h2>
          <button className="close-button" onClick={onClose}>
            <span>×</span>
          </button>
        </div>
        
        <form onSubmit={handleSubmit} className="modal-body">

          {loading && <div className="loading">Загрузка категорий...</div>}
          {error && <div className="error-message">Ошибка загрузки категорий: {error}</div>}
          
          <div className="form-group">
            <div>
              <span>Ежемесячный бюджет</span>
            </div>
            <input
              id="amount"
              type="number"
              step="0.01"
              min="0.01"
              placeholder="0.00"
              value={amount}
              onChange={(e) => setAmount(e.target.value)}
              className={errors.amount ? 'input-error' : ''}
              disabled={isSubmitting || loading}
            />
            {errors.amount && <p className="error-message">{errors.amount}</p>}
          </div>
          
          <div className="form-group">
            <span>Выберите категорию</span>
            <div className="category-grid">
              {categories.map((category: any) => {
                const isSelected = selectedCategoryId === category.categoryId;
                return (
                  <div
                    key={category.categoryId}
                    className={`category-item ${isSelected ? 'selected' : ''}`}
                    title={category.name}
                    onClick={() => !isSubmitting && !loading && setSelectedCategoryId(category.categoryId)}
                  >
                    <div className="category-icon" style={{ backgroundColor: `${getCategoryColor(category.name)}15`, color: getCategoryColor(category.name) }}>
                      {getCategoryIcon(category.name)}
                    </div>
                    <div className="category-name">{category.name}</div>
                  </div>
                );
              })}
            </div>
            {errors.category && <p className="error-message">{errors.category}</p>}
          </div>
          {errors.submit && <p className="error-message">{errors.submit}</p>}
          <div className="modal-actions">
            <button
            type="button"
              className="cancel-button"
              onClick={onClose}
              disabled={isSubmitting}
            >
              Отмена
            </button>
            <button
              type="submit"
              className="add-button"
              disabled={isSubmitting || loading || !selectedCategoryId || !amount}
            >
              {isSubmitting || budgetMutationLoading ? 'Сохранение...' : mode === 'create' ? 'Добавить бюджет' : 'Сохранить изменения'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddBudgetModal;