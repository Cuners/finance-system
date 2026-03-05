import type { BudgetDto, BudgetStatus, BudgetSummary } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = 'http://localhost:5000/api/budget';
export const budgetService = {
    getBudget(): Promise<BudgetDto[]> {
        return httpService.get(`${API_BASE}/Budget`);
    },
    getBudgetsStatus(): Promise<BudgetStatus[]> {
        return httpService.get<{budgetStatuses:BudgetStatus[]}>(`${API_BASE}/Budget/status`)
            .then(response => response.budgetStatuses);
    },
    getBudgetById(id:number): Promise<BudgetDto> {
        return httpService.get<{budget:BudgetDto}>(`${API_BASE}/Budget/${id}`)
            .then(response => response.budget);
    },
    getBudgetsSummary(): Promise<BudgetSummary> {
        return httpService.get(`${API_BASE}/Budget/summary`);
    },
    create(data: any): Promise<BudgetDto> {
        return httpService.post(`${API_BASE}/Budget`, data);
    },
    update(data: any): Promise<BudgetDto> {
        return httpService.put(`${API_BASE}/Budget`, data);
    },
    delete(id: number): Promise<void> {
        return httpService.delete(`${API_BASE}/Budget/${id}`);
    }
};