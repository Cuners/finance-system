import type { AccountDto } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = 'http://localhost:5000/api/budget/Account';

export const accountService = {
  getBalance(): Promise<number> {
    return httpService.get<{ value: number }>(`${API_BASE}/balance`)
      .then(res => res.value);
  },
  getAccounts(): Promise<AccountDto[]>{
    return httpService.get(`${API_BASE}`);
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