import React from 'react';
import './TransactionRow.css';

interface Transaction {
  id: string;
  title: string;
  category: string;
  date: string;
  status: 'completed' | 'pending' | 'failed';
  amount: number;
  type: 'income' | 'expense';
}

const TransactionRow: React.FC<Transaction> = ({
  title,
  category,
  date,
  status,
  amount,
  type,
}) => {
  const formattedAmount = new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);

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
        <button className="action-btn">✏️</button>
        <button className="action-btn">🗑️</button>
      </td>
    </tr>
  );
};

export default TransactionRow;