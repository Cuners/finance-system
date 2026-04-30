import './TransactionItem.css';
import { getCategoryColor } from '../../../Utils/CategoryIcons';
import { formatCurrency } from '../../../Utils/formatUtils';
interface Transaction {
  title?: string;
  category: string;
  type?: 'income' | 'expense';
  variant?: 'default' | 'expense-overview';
  amount: number;
  date: string;
  icon: React.ReactNode;
}

const TransactionItem = ({
  title,
  category,
  type,
  variant = "default",
  amount,
  date,
  icon,
}: Transaction) => {
    const displayAmount = variant === "expense-overview" 
    ? Math.abs(amount) 
    : amount;
  const formattedAmount = formatCurrency(displayAmount, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
  const displayTitle = title || category;
  const color = getCategoryColor(category);
  
  return (
    <div className={`transaction-item ${type} ${variant === "expense-overview" ? "expense-overview" : ''}`}>
      <div className="transaction-icon" style={{ backgroundColor: `${color}15`, color: color}}>{icon}</div>
      <div className="transaction-info">
        <div className="transaction-title">{displayTitle}</div>
        {variant === 'expense-overview' ? (
          <div className="transaction-meta">
            <span className="transaction-date">{date}</span>
          </div>
          ) : (
          <div className="transaction-meta">
            <span className="category">{category}</span>
            <span className="date">{date}</span>
          </div>
        )}
      </div>
      <div className={`transaction-amount ${type}`}>
        {formattedAmount}
      </div>
    </div>
  );
};

export default TransactionItem;