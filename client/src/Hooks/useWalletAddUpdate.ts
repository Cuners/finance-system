import { useState } from 'react';
import { accountService } from '../Services/FinanceService/accountService';

export const useWalletAddUpdate = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const createWallet = async (data: {
    name: string;
    balance: number;
    note: string;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const walletData = {
        Name: data.name,
        Balance: data.balance,
        Note: data.note,
      };
      
      const result = await accountService.create(walletData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to create wallet';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const updateWallet = async (data: {
    accountId: number;
    name: string;
    balance: number;
    note: string;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const walletData = {
        Id: data.accountId,
        Name: data.name,
        Balance: data.balance,
        Note: data.note,
      };
      
      const result = await accountService.update(walletData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to update wallet';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { 
    createWallet, 
    updateWallet, 
    loading, 
    error 
  };
};