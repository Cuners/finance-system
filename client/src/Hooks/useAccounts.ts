import { useState, useEffect } from 'react';

import { type AccountDto } from '../Types';
import { accountService } from '../Services/FinanceService/accountService';

export const useRecentAccounts = () => {
  const [accounts, setAccounts] = useState<AccountDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadAccounts = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await accountService.getAccounts();
        setAccounts(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load accounts');
        console.error('accounts load error:', err);
      } finally {
        setLoading(false);
      }
    };

    loadAccounts();
  }, []);

  return { accounts, loading, error };
};