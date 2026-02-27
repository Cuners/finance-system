// hooks/useExpenseData.ts
import { useState, useEffect } from 'react';
import { transactionService } from '../Services/FinanceService/transactionService';

export const useTopExpenses = () => {
  const [expenses, setExpenses] = useState<Array<{
    categoryName: string;
    amount: number;
    date: string;
  }>>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadExpenses = async () => {
      try {
        setLoading(true);
        setError(null);
        
        const data = await transactionService.getTransactions({
          type: 'expense',
          sortBy: 'amount',
          sortOrder: 'desc',
        });
        
        const formattedExpenses = data.map(t => ({
          categoryName: t.categoryName,
          amount: t.amount,
          date: new Date(t.date).toLocaleDateString('en-US', { 
            month: 'short', 
            day: 'numeric' 
          })
        }));
        
        setExpenses(formattedExpenses);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load expenses');
      } finally {
        setLoading(false);
      }
    };

    loadExpenses();
  }, []);

  return { expenses, loading, error };
};

export const useUpcomingBills = () => {
  const [bills, setBills] = useState<Array<{
    categoryName: string;
    amount: number;
    date: string;
  }>>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadBills = async () => {
      try {
        setLoading(true);
        setError(null);
        
        const data = await transactionService.getTransactions({
          type: 'expense',
          sortBy: 'date',
          sortOrder: 'asc',
        });
        
        const formattedBills = data.map(t => ({
          categoryName: t.categoryName,
          amount: t.amount,
          date: new Date(t.date).toLocaleDateString('en-US', { 
            month: 'short', 
            day: 'numeric' 
          })
        }));
        
        setBills(formattedBills);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load bills');
      } finally {
        setLoading(false);
      }
    };

    loadBills();
  }, []);

  return { bills, loading, error };
};