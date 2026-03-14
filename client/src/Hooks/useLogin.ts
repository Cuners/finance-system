// hooks/useLogin.ts
import { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // 🔥 react-router-dom v6+
import { authService } from '../Services/AuthService/authService';
import type { LoginRequest } from '../Types/index';

export interface UseLoginResult {
  isLoading: boolean;
  error: string | null;
  login: (data: LoginRequest) => Promise<void>;
}

export const useLogin = ()=> {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const login = async (data: LoginRequest)=> {
    setIsLoading(true);
    setError(null);

    try {
      const result = await authService.login(data);
      navigate('/dashboard', { replace: true });
      return result;
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Ошибка входа';
      setError(message);
      throw err; 
    } finally {
      setIsLoading(false);
    }
  };

  return { isLoading, error, login };
};