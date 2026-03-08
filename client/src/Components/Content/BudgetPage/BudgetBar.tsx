import React, { useState } from 'react';
import "./BudgetBar.css";
import { getCategoryIcon, getCategoryColor } from '../../../Utils/CategoryIcons';
import { useBudgetDelete } from '../../../Hooks/useBudgetDelete';
import AddBudgetModal from './BudgetAddUpdate';
import { useBudgetById } from '../../../Hooks/useBudgetById'; 

interface BudgetBarProps {
  id: number;
  categoryName: string;
  spent: number;
  total: number;
  usedPercent: number;
  remaining: number;
  onEdit?: () => void;
  onDelete?: (id: number) => void;
}

const BudgetBar = ({
  id,
  categoryName,
  spent,
  total,
  usedPercent,
  remaining,
  onEdit,
  onDelete
}: BudgetBarProps) => {
  
    const { deleteBudget, loading } = useBudgetDelete();
  
    const formatCurrency = (value: number) =>
      new Intl.NumberFormat('ru-RU', {
      style: 'currency',
      currency: 'RUB'
      }).format(value)
     const formattedPercent = Math.min(100, usedPercent).toFixed(1);
       const icon = getCategoryIcon(categoryName);
      const color = getCategoryColor(categoryName);
      
    const [showDropdown, setShowDropdown] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false); 
    const { data: budgetData, loading: budgetLoading, error } = useBudgetById(showEditModal ? id : null);
    
  const toggleDropdown = () => {
    setShowDropdown(!showDropdown);
  };
  
  const handleEdit = () => {
    setShowEditModal(true); 
    setShowDropdown(false);
  };
  
  const handleCloseModal = () => {
    setShowEditModal(false);
  };
  
  const handleSaveSuccess = () => {
    if (onEdit) onEdit(); 
    setShowEditModal(false);
  };
  
  const handleDelete = async () => {
    try {
      await deleteBudget(id);
      if (onDelete) onDelete(id);
    } catch (error) {
      console.error('Error deleting budget:', error);
    } finally {
      setShowDropdown(false);
    }
  };

  const handleClickOutside = (e: MouseEvent) => {
    const target = e.target as HTMLElement;
    if (!target.closest('.dropdown-menu') && !target.closest('.dropdown-toggle')) {
      setShowDropdown(false);
    }
  };
  
  React.useEffect(() => {
    if (showDropdown) {
      document.addEventListener('click', handleClickOutside);
    } else {
      document.removeEventListener('click', handleClickOutside);
    }
    
    return () => {
      document.removeEventListener('click', handleClickOutside);
    };
  }, [showDropdown]);
  
  return (
    <div className="budgetPage-budget-bar">
      <div className="bar-header">
        <div className="bar-icon" style={{ backgroundColor: `${color}15`, color: color }}>
          {icon}
        </div>
        <div className="bar-title">
          <span className="category-name">{categoryName}</span>
          <span className="budget-amount">
            {formatCurrency(spent)} из {formatCurrency(total)}
          </span>
        </div>
        <div className="dropdown-container">
          <button
            className="dropdown-toggle"
            onClick={toggleDropdown}
          >
            ...
          </button>
          {showDropdown && (
            <div className="dropdown-menu">
              <button className="dropdown-item" onClick={handleEdit}>
                Редактировать
              </button>
              <button className="dropdown-item" onClick={handleDelete}>
                Удалить
              </button>
            </div>
          )}
        </div>
      </div>

      <div className="progress-container">
        <div className="progress-bar">
          <div
            className="progress-fill"
            style={{
              width: `${Math.min(100, usedPercent)}%`,
              backgroundColor: color
            }}
          ></div>
        </div>
        <div className="progress-info">
          <span className={`used-percent ${usedPercent > 100 ? 'over-limit' : ''}`}>
            {usedPercent > 100
              ? `${formattedPercent}% (превышен лимит)`
              : `${formattedPercent}% использовано`}
          </span>
          <span className={`remaining-amount ${usedPercent > 100 ? 'negative' : ''}`}>
            {formatCurrency(remaining)} осталось
          </span>
        </div>
      </div>
      
      <AddBudgetModal
        isOpen={showEditModal && (!budgetLoading || budgetData)}
        onClose={handleCloseModal}
        mode="edit"
        budgetData={budgetData ? {
          budgetId: budgetData.budgetId,
          categoryId: budgetData.categoryId,
          categoryName: budgetData.categoryName,
          amount: budgetData.limitAmount
        } : undefined}
        onSuccess={handleSaveSuccess}
      />
    </div>
  );
};

export default BudgetBar;