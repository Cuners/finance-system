import { useState, useEffect } from 'react';
import { budgetService } from '../Services/FinanceService/budgetService';
import { type BudgetDto } from '../Types';

export const useRecentBudgets = () => {
  const [budgets, setBudgets] = useState<BudgetDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadBudgets = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await budgetService.getBudget();
        setBudgets(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load budgets');
        console.error('Budgets load error:', err);
      } finally {
        setLoading(false);
      }
    };

    loadBudgets();
  }, []);

  return { budgets, loading, error };
};