export interface DashboardData {
  totalBalance: number;
  totalIncome: number;
  totalExpenses: number;
  savings: number;
}

export interface TransactionSummaryDto {
  transactions: {
    netBalance: number;
    totalExpenses: number;
    totalIncome: number;
  };
}

export interface TransactionDto{
  transactionId: number;
  accountId:number;
  accountName:string;
  categoryId:number;
  categoryName:string;
  amount:number;
  date:Date;
  note:string;
}

export interface AccountSummaryDto{
  accountId:number;
  name:string;
  balance:number;
  note:string;
  income:number;
  expense:number;
  count:number;
}

export interface AccountDto{
  accountId:number;
  name:string;
  balance:number;
  note:string;
}

export interface BudgetDto{
  budgetId:number;
  categoryId:number;
  categoryName: string,
  name: string;
  limitAmount:number;
  date: Date;
}
export interface BudgetStatus{
  budgetId:number;
  categoryName: string,
  limitAmount:number;
  totalSpent:number;
  procentSpent:number;
  remaining:number;
  period: Date;
}
export interface BudgetSummary{
  budgetSummaryDto: {
    totalBudget:number;
    totalSpent:number;
    remaining:number;
    procentSpent:number
  }
}
export interface CategoryDto{
  categoryId:number;
  name:string;
}
export interface LoginRequest{
  username:string;
  password:string;
}

export interface UserDto{
  login:string;
  password:string;
  email:string;
}

export interface Notification {
  notificationId: number;
  userId: number;
  type: 'transaction' | 'budget' | 'auth' | 'system';
  typeId: number;
  title: string;
  message: string;
  isRead: boolean;
  createdAt: string; 
  emailSentAt?: string | null;
  data?: string | null; 
}

export interface NotificationData {
  type: string;
  notificationId: number;
  data?: Record<string, unknown>;
}

export interface UseNotificationsReturn {
  notifications: Notification[];
  unreadCount: number;
  isConnected: boolean;
  isLoading: boolean;
  error: string | null;
  loadNotifications: () => Promise<void>;
  loadUnreadCount: () => Promise<void>;
  markAsRead: (id: number) => Promise<void>;
  markAllAsRead: () => Promise<void>;
  refresh: () => void;
}