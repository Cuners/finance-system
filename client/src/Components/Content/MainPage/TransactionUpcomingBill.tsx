import styles from './UpcomingBills.module.css';
import TransactionItem from './TransactionItem';
import { getCategoryIcon, getCategoryColor } from '../../../Utils/CategoryIcons';
import { useRecentTransactions } from '../../../Hooks/useTransactions';
const TransactionUpcomingBills = () => {
  const { data, loading, error } = useRecentTransactions({ 
    type: 'expense',
    sortBy: 'date',
    sortOrder: 'asc'
  });
  
    const upcomingBills = data.slice(0, 5);
  if (loading) {
    return <div className="top-expenses">Loading top expenses...</div>;
  }

  if (error) {
    return <div className="top-expenses">Error loading expenses: {error}</div>;
  }
  return (
    <div className={styles.upcomingBills}>
      <h3 className={styles.sectionTitle}>Недавние транзакции</h3>
      <div className={styles.billsList}>
        {upcomingBills.map((transaction) => (
          <TransactionItem
            key={transaction.transactionId}
            category={transaction.categoryName}
            amount={transaction.amount}
            date={new Date(transaction.date).toLocaleDateString('ru-RU', { month: 'short', day: 'numeric' })}
            variant='expense-overview'
            icon={getCategoryIcon(transaction.categoryName)}
          />
        ))}
      </div>
    </div>
  );
};

export default TransactionUpcomingBills;