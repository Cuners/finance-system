import type { LoginRequest } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = '/api/auth/Auth';

export const authService = {
  login(data: any): Promise<LoginRequest> {
    return httpService.post(`${API_BASE}/login`, data);
  }, 
  registration(data: any): Promise<LoginRequest> {
    return httpService.post(`${API_BASE}/registration`, data);
  }, 
  logout(data: any): Promise<void> {
    return httpService.post(`${API_BASE}/logout`, data);
  }, 
  delete(id: number): Promise<void> {
    return httpService.delete(`${API_BASE}/${id}`);
  },
};