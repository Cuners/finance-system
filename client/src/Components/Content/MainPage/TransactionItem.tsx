import './TransactionItem.css';

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
  type = 'expense',
  variant = 'default',
  amount,
  date,
  icon,
}: Transaction) => {
    const displayAmount = variant === 'expense-overview' 
    ? Math.abs(amount) 
    : amount;
  const formattedAmount = new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(displayAmount);
  const displayTitle = title || category;
  return (
    <div className={`transaction-item ${type} ${variant === 'expense-overview' ? 'expense-overview' : ''}`}>
      <div className="transaction-icon">{icon}</div>
      <div className="transaction-info">
        <div className="transaction-title">{displayTitle}</div>
        {variant === 'expense-overview' ? (
          <div className="transaction-date">{date}</div>
          ) : (
          <div className="transaction-meta">
            <span className="category">{category}</span>
            <span className="date">• {date}</span>
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