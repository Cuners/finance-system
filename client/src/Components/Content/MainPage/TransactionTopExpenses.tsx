import TransactionItem from './TransactionItem';
import { getCategoryIcon, getCategoryColor } from '../../../Utils/CategoryIcons';
import { useRecentTransactions } from '../../../Hooks/useTransactions';
const TransactionTopExpenses = () => {
  const { data: transactions, loading, error } = useRecentTransactions({ 
    type: 'expense',
    sortBy: 'amount',
    sortOrder: 'desc'
  });
  const topExpenses = transactions.slice(0, 5);
  if (loading) {
    return <div className="top-expenses">Loading top expenses...</div>;
  }

  if (error) {
    return <div className="top-expenses">Error loading expenses: {error}</div>;
  }
  return (
    <div className="top-expenses">
      <h3 className="section-title">Высшие расходы</h3>
      <div className="expenses-list">
        {topExpenses.map((transaction) => (
          <TransactionItem
            key={transaction.transactionId}
            category={transaction.categoryName}
            amount={transaction.amount}
            date={new Date(transaction.date).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })}
            icon={getCategoryIcon(transaction.categoryName)}
            variant="expense-overview"
          />
        ))}
      </div>
    </div>
  );
};
export default TransactionTopExpenses;