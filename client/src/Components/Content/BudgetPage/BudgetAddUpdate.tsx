// AddBudgetModal.tsx
import React, { useState, useEffect } from 'react';
import './AddBudgetModal.css';
import { getCategoryIcon } from '../../../Utils/CategoryIcons';

interface BudgetCategory {
  id: string;
  name: string; 
  icon: React.ReactNode;
}

interface AddBudgetModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (data: {
    categoryId: string;
    categoryName: string;
    amount: number;
  }) => void;
  mode: 'create' | 'edit';
  budgetData?: {
    categoryId: string;
    categoryName: string;
    amount: number;
  };
}

const AddBudgetModal = ({
  isOpen,
  onClose,
  onSubmit,
  mode,
  budgetData
}: AddBudgetModalProps) => {
  const [amount, setAmount] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<BudgetCategory | null>(null);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const budgetCategories: BudgetCategory[] = [
    { 
      id: 'food', 
      name: 'Продукты', 
      icon: getCategoryIcon('Продукты')
    },
    { 
      id: 'transport', 
      name: 'Транспорт', 
      icon: getCategoryIcon('Транспорт')
    },
    { 
      id: 'entertainment', 
      name: 'Развлечения', 
      icon: getCategoryIcon('Развлечения')
    },
    { 
      id: 'health', 
      name: 'Здоровье', 
      icon: getCategoryIcon('Здоровье')
    },
    { 
      id: 'utilities', 
      name: 'Коммунальные услуги', 
      icon: getCategoryIcon('Коммунальные услуги')
    },
    { 
      id: 'shopping', 
      name: 'Покупки', 
      icon: getCategoryIcon('Покупки')
    },
    { 
      id: 'travel', 
      name: 'Путешествия', 
      icon: getCategoryIcon('Путешествия')
    },
    { 
      id: 'education', 
      name: 'Образование', 
      icon: getCategoryIcon('Образование')
    }
  ];

  // Инициализация при редактировании
  useEffect(() => {
    if (mode === 'edit' && budgetData) {
      setAmount(budgetData.amount.toString());
      
      // Найти категорию по имени
      const category = budgetCategories.find(c => 
        c.name.toLowerCase() === budgetData.categoryName.toLowerCase() ||
        budgetData.categoryName.toLowerCase().includes(c.name.toLowerCase())
      );
      
      setSelectedCategory(category || null);
    }
  }, [mode, budgetData]);

  const validateForm = () => {
    const newErrors: { [key: string]: string } = {};
    
    const parsedAmount = parseFloat(amount);
    if (!amount || isNaN(parsedAmount) || parsedAmount <= 0) {
      newErrors.amount = 'Введите корректную сумму';
    }
    
    if (!selectedCategory) {
      newErrors.category = 'Выберите категорию';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    setIsSubmitting(true);
    
    onSubmit({
      categoryId: selectedCategory!.id,
      categoryName: selectedCategory!.name,
      amount: parseFloat(amount)
    });
    
    // Сброс состояния после успешной отправки
    setTimeout(() => {
      setIsSubmitting(false);
      setAmount('');
      setSelectedCategory(null);
      setErrors({});
      onClose();
    }, 500);
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
          {/* Monthly Budget Field */}
          <div className="form-group">
            <label htmlFor="amount">Ежемесячный бюджет</label>
            <input
              id="amount"
              type="number"
              step="0.01"
              min="0.01"
              placeholder="0.00"
              value={amount}
              onChange={(e) => setAmount(e.target.value)}
              className={errors.amount ? 'input-error' : ''}
            />
            {errors.amount && <p className="error-message">{errors.amount}</p>}
          </div>
          
          {/* Category Selection */}
          <div className="form-group">
            <label>Выберите категорию</label>
            <div className="category-grid">
              {budgetCategories.map(category => {
                const isSelected = selectedCategory?.id === category.id;
                return (
                  <div 
                    key={category.id} 
                    className={`category-item ${isSelected ? 'selected' : ''}`}
                    title={category.name}
                    onClick={() => setSelectedCategory(category)}
                  >
                    <div className="category-icon">
                      {category.icon}
                    </div>
                    <div className="category-name">{category.name}</div>
                  </div>
                );
              })}
            </div>
            {errors.category && <p className="error-message">{errors.category}</p>}
          </div>
          
          {/* Action Buttons */}
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
              disabled={isSubmitting}
            >
              {isSubmitting ? 'Сохранение...' : mode === 'create' ? 'Добавить бюджет' : 'Сохранить изменения'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddBudgetModal;