import { useState, useEffect } from 'react';
import { budgetService } from '../Services/FinanceService/budgetService';
import { type BudgetStatus } from '../Types';

export const useBudgetsStatus = () => {
  const [budgets, setBudgets] = useState<BudgetStatus[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadBudgets = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await budgetService.getBudgetsStatus();
        setBudgets(data || []);
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