import type { AccountDto, AccountSummaryDto } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = '/api/budget/Account';

export const accountService = {
  getBalance(): Promise<number> {
    return httpService.get<{ value: number }>(`${API_BASE}/balance`)
      .then(res => res.value);
  },
  getAccounts(): Promise<AccountSummaryDto[]>{
    return httpService.get<{accounts:AccountSummaryDto[]}>(`${API_BASE}`)
    .then(response=>response.accounts);
  },
  getAccountById(id:number): Promise<AccountDto>{
    return httpService.get<{account:AccountDto}>(`${API_BASE}/${id}`)
    .then(response=>response.account);
  },
  create(data: any): Promise<AccountDto> {
    return httpService.post(API_BASE, data);
  }, 
  update(data: any): Promise<AccountDto> {
    return httpService.put(API_BASE, data);
  }, 
  delete(id: number): Promise<void> {
    return httpService.delete(`${API_BASE}/${id}`);
  },
};