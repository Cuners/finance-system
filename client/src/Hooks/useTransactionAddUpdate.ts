import { useState } from 'react';
import { transactionService } from '../Services/FinanceService/transactionService';

export const useTransactionAddUpdate = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const createTransaction = async (data: {
    accountId: number;
    accountName: string;
    categoryId: number;
    categoryName: string;
    amount: number;
    date: Date;
    note: string;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const transactionData = {
        AccountId: data.accountId,
        AccountName: data.accountName,
        CategoryId: data.categoryId,
        CategoryName: data.categoryName,
        Amount: data.amount,
        Date: data.date,
        Note: data.note,
      };
      
      const result = await transactionService.create(transactionData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to create transaction';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const updateTransaction = async (data: {
    transactionId: number;
    accountId: number;
    accountName: string;
    categoryId: number;
    categoryName: string;
    amount: number;
    date: Date;
    note: string;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const transactionData = {
        Id: data.transactionId,
        AccountId: data.accountId,
        AccountName: data.accountName,
        CategoryId: data.categoryId,
        CategoryName: data.categoryName,
        Amount: data.amount,
        Date: data.date,
        Note: data.note,
      };
      
      const result = await transactionService.update(transactionData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to update transaction';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { 
    createTransaction, 
    updateTransaction, 
    loading, 
    error 
  };
};