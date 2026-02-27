// BudgetOverview.tsx
import { useBudgetsStatus } from '../../../Hooks/useBudgetsStatus';
import BudgetBar from './BudgetBar';

const BudgetCategories = () => {
  const { budgets, loading, error } = useBudgetsStatus();

  if (loading) return <div className="budget-overview">Загрузка бюджетов...</div>;
  if (error) return <div className="budget-overview">Ошибка: {error}</div>;

  return (
    <div className="budget-overview">
      <h2 className="section-title">Бюджеты на февраль</h2>
      
      {budgets.length === 0 ? (
        <div className="no-budgets">
          <p>У вас пока нет бюджетов</p>
          <button className="create-budget-btn">+ Создать бюджет</button>
        </div>
      ) : (
        budgets.map(budget => (
          <BudgetBar
            key={budget.budgetId}
            categoryName={budget.categoryName}
            spent={budget.totalSpent}
            total={budget.limitAmount}
            usedPercent={budget.procentSpent} 
            remaining={budget.remaining}      
          />
        ))
      )}  
     <div>
        </div>
    </div>
  );
};

export default BudgetCategories;