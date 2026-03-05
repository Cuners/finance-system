import { useState } from 'react';
import { budgetService } from '../Services/FinanceService/budgetService';

export const useBudgetDelete = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const deleteBudget = async (id: number) => {
    setLoading(true);
    setError(null);
    
    try {
      await budgetService.delete(id);
      return true;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to delete budget';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { 
    deleteBudget, 
    loading, 
    error 
  };
};