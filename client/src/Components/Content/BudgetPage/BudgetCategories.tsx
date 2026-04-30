// BudgetOverview.tsx
import { useState, useEffect } from 'react';
import { useBudgetsStatus } from '../../../Hooks/useBudgetsStatus';
import BudgetBar from './BudgetBar';
import { useBudgetDelete } from '../../../Hooks/useBudgetDelete';

const BudgetCategories = () => {
  const { budgets, loading, error } = useBudgetsStatus();
  const { deleteBudget } = useBudgetDelete();
  const [localBudgets, setLocalBudgets] = useState(budgets);
  useEffect(() => {
    setLocalBudgets(budgets);
  }, [budgets]);

  const handleDelete = async (id: number) => {
    try {
      await deleteBudget(id);

      setLocalBudgets(prev => prev.filter(budget => budget.budgetId !== id));
    } catch (error) {
      console.error('Error deleting budget:', error);
    }
  };

  if (loading) return <div className="budget-overview">Загрузка бюджетов...</div>;
  if (error) return <div className="budget-overview">Ошибка: {error}</div>;

  return (
    <div className="budget-overview">
      <h2 className="section-title">Бюджеты на февраль</h2>
      
      {localBudgets.length === 0 ? (
        <div className="no-budgets">
          <p>У вас пока нет бюджетов</p>
          <button className="create-budget-btn">+ Создать бюджет</button>
        </div>
      ) : (
        localBudgets.map(budget => (
          <BudgetBar
            key={budget.budgetId}
            id={budget.budgetId}
            categoryName={budget.categoryName}
            spent={budget.totalSpent}
            total={budget.limitAmount}
            usedPercent={budget.procentSpent}
            remaining={budget.remaining}
            onDelete={handleDelete}
          />
        ))
      )}
     <div>
        </div>
    </div>
  );
};

export default BudgetCategories;