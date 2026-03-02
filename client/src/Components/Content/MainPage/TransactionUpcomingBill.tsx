import TransactionItem from './TransactionItem';
import { getCategoryIcon} from '../../../Utils/CategoryIcons';
import { useRecentTransactions } from '../../../Hooks/useTransactions';
const TransactionUpcomingBills = () => {
  const { data, loading, error } = useRecentTransactions({ 
    type: "income",
    sortBy: "date",
    sortOrder: "asc"
  });
  
  const upcomingBills = data.slice(0, 5);
  if (loading) {
    return <div className="top-expenses">Loading top expenses...</div>;
  }

  if (error) {
    return <div className="top-expenses">Error loading expenses: {error}</div>;
  }
  return (
    <div className="top-expenses">
      <h3 className="section-title">Высшие доходы</h3>
      <div className="expenses-list">
        {upcomingBills.map((transaction) => (
          <TransactionItem
            key={transaction.transactionId}
            category={transaction.categoryName}
            amount={transaction.amount}
            date={new Date(transaction.date).toLocaleDateString("ru-RU", { month: "long", day: "numeric" })}
            variant="expense-overview"
            icon={getCategoryIcon(transaction.categoryName)}
            type="income"
          />
        ))}
      </div>
    </div>
  );
};

export default TransactionUpcomingBills;