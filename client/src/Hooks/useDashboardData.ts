import { useState, useEffect } from 'react';
import { accountService } from '../Services/FinanceService/accountService';
import { transactionService } from '../Services/FinanceService/transactionService';
import { type DashboardData } from '../Types';

export const useDashboardData = (year: number, month: number) => {
  const [data, setData] = useState<DashboardData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadDashboard = async () => {
      try {
        setLoading(true);
        setError(null);

        // Запускаем запросы параллельно
        const [balance, summary] = await Promise.all([
          accountService.getBalance(),
          transactionService.getSummary(year, month)
        ]);

        setData({
          totalBalance: balance,
          totalIncome: summary.transactions.totalIncome,
          totalExpenses: summary.transactions.totalExpenses,
          savings: summary.transactions.netBalance
        });
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load dashboard');
        console.error('Dashboard load error:', err);
      } finally {
        setLoading(false);
      }
    };

    loadDashboard();
  }, [year, month]);

  return { data, loading, error };
};