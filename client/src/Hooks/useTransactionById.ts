import { useState, useEffect } from 'react';
import { transactionService } from '../Services/FinanceService/transactionService';

export const useTransactionById = (id: number | null) => {
  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id === null) {
      setLoading(false);
      return;
    }

    const fetchData = async () => {
      try {
        setLoading(true);
        const result = await transactionService.getTransactionById(id);
        setData(result);
        setError(null);
      } catch (err) {
        console.error('Error fetching budget by ID:', err);
        setError(err instanceof Error ? err.message : 'Failed to fetch budget');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  return { data, loading, error };
};