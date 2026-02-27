import "./BudgetBar.css";
import { getCategoryIcon, getCategoryColor } from '../../../Utils/CategoryIcons';
interface BudgetBarProps {
  categoryName: string;
  spent: number;       
  total: number;       
  usedPercent: number; 
  remaining: number;   
}

const BudgetBar = ({ 
  categoryName, 
  spent, 
  total, 
  usedPercent, 
  remaining
}: BudgetBarProps) => {

  const formatCurrency = (value: number) => 
    new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB'
    }).format(value)
   const formattedPercent = Math.min(100, usedPercent).toFixed(1);
     const icon = getCategoryIcon(categoryName);
    const color = getCategoryColor(categoryName);
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
    </div>
  );
};

export default BudgetBar;