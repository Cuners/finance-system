import { useState, useEffect } from 'react';
import { categoryService } from '../Services/FinanceService/categoryService';
import { type CategoryDto } from '../Types';

export const useRecentTransactions = () => {
  const [transactions, setTransactions] = useState<CategoryDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadTransactions = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await categoryService.getAll();
        setTransactions(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load transactions');
        console.error('Transactions load error:', err);
      } finally {
        setLoading(false);
      }
    };

    loadTransactions();
  }, []);

  return { transactions, loading, error };
};