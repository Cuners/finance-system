import { httpService } from '../httpService';
import { type TransactionSummaryDto, type TransactionDto } from '../../Types';

const API_BASE = 'http://localhost:5000/api/budget/Transaction';
export interface GetTransactionsParams {
  accountId?:number,
  type?: 'income' | 'expense';
  startDate?: Date;
  endDate?: Date;
  sortBy?: 'date' | 'amount' | 'category' | 'account';
  sortOrder?: 'asc' | 'desc';
}
const formatDate = (date: Date): string => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
};
export const transactionService = {
  // GET /api/transactions/values?year=...&month=...
  getSummary(year: number, month: number): Promise<TransactionSummaryDto> {
    return httpService.get(`${API_BASE}/values?year=${year}&month=${month}`);
  },
  // GET /api/transactions
  getTransactions(params: GetTransactionsParams = {}): Promise<TransactionDto[]> {
    const searchParams = new URLSearchParams();
        if(params.accountId){
          searchParams.append('accountId',params.accountId.toString());
        }
        if (params.type) {
          searchParams.append('type', params.type);
        }
        if (params.startDate) {
          searchParams.append('startDate', formatDate(params.startDate));
        }
        if (params.endDate) {
          searchParams.append('endDate', formatDate(params.endDate));
        }
        if (params.sortBy) {
          searchParams.append('sortBy', params.sortBy);
        }
        if (params.sortOrder) {
          searchParams.append('sortOrder', params.sortOrder);
        }
        return httpService.get<{transactions: TransactionDto[]}>(`${API_BASE}?${searchParams.toString()}`)
        .then(response => response.transactions);
  },
  getAll(): Promise<TransactionDto[]> {
    return httpService.get<{ transactions: TransactionDto[] }>(`${API_BASE}`)
      .then(response => response.transactions);
  },

  // POST /api/transactions
  create(data: any): Promise<TransactionDto> {
    return httpService.post(API_BASE, data);
  }, 

  update(data: any): Promise<TransactionDto> {
    return httpService.put(API_BASE, data);
  }, 

  // DELETE /api/transactions/123
  delete(id: number): Promise<void> {
    return httpService.delete(`${API_BASE}/${id}`);
  } 
};