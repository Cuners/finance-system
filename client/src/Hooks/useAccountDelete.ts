import { useState } from 'react';
import { accountService } from '../Services/FinanceService/accountService';

export const useAccountDelete = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const deleteAccount = async (id: number) => {
    setLoading(true);
    setError(null);
    
    try {
      await accountService.delete(id);
      return true;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to delete account';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { 
    deleteAccount, 
    loading, 
    error 
  };
};