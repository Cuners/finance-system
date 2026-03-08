import { useState } from 'react';
import { transactionService } from '../Services/FinanceService/transactionService';

export const useTransactionDelete = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const deleteTransaction = async (id: number) => {
    setLoading(true);
    setError(null);
    
    try {
      await transactionService.delete(id);
      return true;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to delete transaction';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { 
    deleteTransaction, 
    loading, 
    error 
  };
};