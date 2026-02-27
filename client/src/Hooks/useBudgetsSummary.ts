import { useState, useEffect } from 'react';
import { budgetService } from '../Services/FinanceService/budgetService';
import { type BudgetSummary } from '../Types';

export const useBudgetsSummary = () => {
  const [budgets, setBudgets] = useState<BudgetSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadBudgets = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await budgetService.getBudgetsSummary();
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