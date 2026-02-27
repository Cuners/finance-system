// TransactionTable.tsx
import React from 'react';
import TransactionRow from './TransactionRow';
import type { Transaction } from './TransactionPage'; // ← импортируем тип
import './TransactionTable.css';

interface TransactionsTableProps {
  transactions: Transaction[];
}

const TransactionsTable: React.FC<TransactionsTableProps> = ({ transactions }) => {
  if (transactions.length === 0) {
    return (
      <div className="empty-state">
         Не найдено транзакций для выбранных фильтров.
      </div>
    );
  }

  return (
    <table className="transactions-table">
      <thead>
        <tr>
          <th className="cell icon"></th>
          <th className="cell">Транзакции</th>
          <th className="cell">Категория</th>
          <th className="cell">Дата</th>
          <th className="cell">Статус</th>
          <th className="cell">Количество</th>
          <th className="cell actions">Действия</th>
        </tr>
      </thead>
      <tbody>
        {transactions.map((t) => (
          <TransactionRow key={t.id} {...t} />
        ))}
      </tbody>
    </table>
  );
};

export default TransactionsTable;