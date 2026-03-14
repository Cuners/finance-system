import { useState, useEffect } from 'react';
import { accountService } from '../Services/FinanceService/accountService';
import type { AccountDto } from '../Types';

export const useAccountById = (id: number | null) => {
  const [data, setData] = useState<AccountDto | null>(null);
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
        const result = await accountService.getAccountById(id);
        setData(result);
        setError(null);
      } catch (err) {
        console.error('Error fetching account by ID:', err);
        setError(err instanceof Error ? err.message : 'Failed to fetch account');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  return { data, loading, error };
};