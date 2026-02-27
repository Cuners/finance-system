import { useState, useEffect } from 'react';
import { transactionService, type GetTransactionsParams } from '../Services/FinanceService/transactionService';
import { type TransactionDto } from '../Types';

export const useRecentTransactions = (params: GetTransactionsParams = {}) => {
  const [data, setData] = useState<TransactionDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadTransactions = async () => {
      try {
        setLoading(true);
        setError(null);
        const result = await transactionService.getTransactions(params);
        setData(result);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load transactions');
        console.error('Transactions load error:', err);
      } finally {
        setLoading(false);
      }
    };

    loadTransactions();
  }, [
    params.type,
    params.startDate?.toISOString(),
    params.endDate?.toISOString(),
    params.sortBy,
    params.sortOrder
  ]);

  return { data, loading, error };
};