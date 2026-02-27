import { useRecentTransactions } from '../../../Hooks/useTransactions';
import TransactionItem from './TransactionItem';
import './RecentTransactions.css';

interface DisplayTransaction {
  id: string;
  title: string;
  category: string;
  type: 'income' | 'expense';
  amount: number;
  date: string;
  icon: string;
}

const RecentTransactions = () => {
  const { data, loading, error } = useRecentTransactions();
  // Преобразуем TransactionDto → DisplayTransaction
  const displayTransactions: DisplayTransaction[] = data.map(t => ({
    id: t.transactionId.toString(),
    title: t.note || 'Без описания',
    category: t.categoryName,
   type: (t.amount >= 0 ? 'income' : 'expense') as 'income' | 'expense',
    amount: Math.abs(t.amount),
    date: new Date(t.date).toLocaleDateString('ru-RU'),
    icon: (t.amount >= 0 ? 'income' : 'expense') === 'income' ? '💼' : '🛒' 
  })).slice(0, 5);

  if (loading) return <div className="recent-transactions">Загрузка...</div>;
  if (error) return <div className="recent-transactions">Ошибка: {error}</div>;

  return (
    <div className="recent-transactions">
      <div className="section-header">
        <h3>Последние транзакции</h3>
        <a href="#" className="view-all">Все</a>
      </div>
      <div className="transactions-list">
        {displayTransactions.map((t) => (
          <TransactionItem
            key={t.id}
            {...t}
          />
        ))}
      </div>
    </div>
  );
};

export default RecentTransactions;