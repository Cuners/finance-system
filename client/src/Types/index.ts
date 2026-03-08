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

export interface AccountDto{
  accountId:number;
  name:string;
  balance:number;
  note:string;
  income:number;
  expense:number;
  count:number;
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