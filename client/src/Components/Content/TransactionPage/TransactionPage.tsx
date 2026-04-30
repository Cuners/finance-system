import  { useState, useMemo } from 'react';
import TransactionsHeader from '../ContentHeader';
import TransactionsTable from './TransactionTable';
import './TransactionPage.css';
import { useRecentTransactions } from '../../../Hooks/useTransactions';
import { getDateRangeForPeriod } from '../../../Utils/dateUtils';
import TransactionCreateUpdate from './TransactionCreateUpdate';
export interface Transaction {
  id: number;
  title: string;
  category: string;
  date: string; 
  status: 'completed' | 'pending' | 'failed';
  amount: number;
  type: 'income' | 'expense';
}
{/*const mockTransactions: Transaction[] = [
  { id: 1, title: 'Salary Deposit', category: 'Income', date: '2026-01-15', status: 'completed', amount: 5500, type: 'income' },
  { id: '2', title: 'Grocery Shopping', category: 'Food & Dining', date: '2026-01-14', status: 'completed', amount: -125.5, type: 'expense' },
  { id: '3', title: 'Electric Bill', category: 'Utilities', date: '2026-01-13', status: 'completed', amount: -89.99, type: 'expense' },
  { id: '4', title: 'Freelance Project', category: 'Income', date: '2026-01-12', status: 'completed', amount: 1200, type: 'income' },
  { id: '5', title: 'Netflix Subscription', category: 'Entertainment', date: '2026-01-11', status: 'completed', amount: -15.99, type: 'expense' },
  { id: '6', title: 'Restaurant Dinner', category: 'Food & Dining', date: '2025-12-20', status: 'completed', amount: -85, type: 'expense' },
  { id: '7', title: 'Gas Station', category: 'Transportation', date: '2025-12-15', status: 'completed', amount: -45, type: 'expense' },
  { id: '8', title: 'Investment Return', category: 'Income', date: '2025-12-10', status: 'completed', amount: 350, type: 'income' },
  { id: '9', title: 'Gym Membership', category: 'Health & Fitness', date: '2026-01-07', status: 'completed', amount: -50, type: 'expense' },
]; */}

const TransactionPage = () => {
  const [filterType, setFilterType] = useState<'all' | 'income' | 'expense'>('all');
  const [period, setPeriod] = useState<'this-month' | 'last-month' | 'this-year'>('this-month');
  const [sortBy, setSortBy] = useState<'date' | 'amount' | 'category'>('date');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');
    const { startDate, endDate } = useMemo(() => {
    return getDateRangeForPeriod(period);
  }, [period]);
    const { data, loading, error } = useRecentTransactions({
        type: filterType === 'all' ? undefined : filterType,
        startDate,  
        endDate,    
        sortBy,
        sortOrder
  });

  const displayTransactions: Transaction[] = useMemo(() => {
    return data.map(t => ({
      id: t.transactionId,
      title: t.note || 'Без описания',
      category: t.categoryName,
      date: new Date(t.date).toLocaleDateString('ru-RU'),
      status: 'completed' as const,
      amount: Math.abs(t.amount),
      type: (t.amount >= 0 ? 'income' : 'expense') as 'income' | 'expense',
    }));
  }, [data]); 
  const [isModalOpen, setIsModalOpen] = useState(false);

    const handleOpenModal = () => {
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
    };
 {/* if (loading) return <div className="recent-transactions">Загрузка...</div>;*/}
  if (error) return <div className="recent-transactions">Ошибка: {error}</div>;
  return (
    <div className="transactions-page">
      <div className="header-controls">
        <TransactionsHeader
          title='Транзакции'
          description='Просматривайте и планируйте все ваши транзакции'/>
        <button className="add-btn" onClick={handleOpenModal}>+ Добавить транзакцию</button>
      </div>
      <div className="table-actions">
        <div className="header-actions">
          <div className="filters">
            <button
              className={`filter-btn ${filterType === 'all' ? 'active' : ''}`}
              onClick={() => setFilterType('all')}
            >
              Все
            </button>
            <button
              className={`filter-btn ${filterType === 'income' ? 'active' : ''}`}
              onClick={() => setFilterType('income')}
            >
              Доходы
            </button>
            <button
              className={`filter-btn ${filterType === 'expense' ? 'active' : ''}`}
              onClick={() => setFilterType('expense')}
            >
              Расходы
            </button>
          </div>

          <select
            value={period}
            onChange={(e) => setPeriod(e.target.value as any)}
            className="period-select"
          >
            <option value="this-month">Текущий месяц</option>
            <option value="last-month">Прошлый месяц</option>
            <option value="this-year">Текущий год</option>
          </select>

          <button className="export-btn">📥 Export</button>
        </div>

        <TransactionsTable transactions={displayTransactions} />
      </div>
        {isModalOpen && (
        <TransactionCreateUpdate
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          mode="create"
          onSuccess={() => {
          }}
        />
      )}
    </div>
  );
};

export default TransactionPage;