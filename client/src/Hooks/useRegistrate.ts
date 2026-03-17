import { useState } from 'react';
import { authService } from '../Services/AuthService/authService';

export const useBudgetAddUpdate = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const createBudget = async (data: {
    login: string;
    password: string;
    email: string;
  }) => {
    setLoading(true);
    setError(null);
    
    try {
      const authData = {
        Login: data.login,
        Password: data.password,
        Email: data.email,
      };
      
      const result = await authService.registration(authData);
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to create account';
      setError(errorMessage);
      throw err;
    } finally {
      setLoading(false);
    }
  };
  return { 
    createBudget, 
    loading, 
    error 
  };
};