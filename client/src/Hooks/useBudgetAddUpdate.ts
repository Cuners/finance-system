import { useState } from 'react';
import { budgetService } from '../Services/FinanceService/budgetService';
import { getLocalDateString } from '../Utils/formatUtils';
export const useBudgetAddUpdate = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const createBudget = async (data: {
    categoryId: number;
    categoryName: string;
    amount: number;
    date:Date;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const budgetData = {
        Name: data.categoryName,
        LimitAmount: data.amount,
        Date: getLocalDateString(data.date),
        CategoryId: data.categoryId,
      };
      
      const result = await budgetService.create(budgetData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to create budget';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };
  const updateBudget = async (data: {
    budgetId: number;
    categoryName: string;
    amount: number;
    categoryId: number;
    date:Date;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const budgetData = {
        BudgetId: data.budgetId,
        Name: data.categoryName,
        LimitAmount: data.amount,
        Date: getLocalDateString(data.date),
        CategoryId: data.categoryId,
      };
      
      const result = await budgetService.update(budgetData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to update budget';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { 
    createBudget, 
    updateBudget, 
    loading, 
    error 
  };
};