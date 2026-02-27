import type { BudgetDto, BudgetStatus, BudgetSummary } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = 'http://localhost:5000/api/budget/Budget';
export const budgetService = {
    getBudget(): Promise<BudgetDto[]> {
        return httpService.get(`${API_BASE}`);
    },
    getBudgetsStatus(): Promise<BudgetStatus[]> {
        return httpService.get<{budgetStatuses:BudgetStatus[]}>(`${API_BASE}/status`)
            .then(response => response.budgetStatuses);
    },
    getBudgetsSummary(): Promise<BudgetSummary> {
        return httpService.get(`${API_BASE}/summary`);
    },
    create(data: any): Promise<BudgetDto[]> {
        return httpService.post(API_BASE, data);
    },
    update(data: any): Promise<BudgetDto[]> {
        return httpService.put(API_BASE, data);
    },
    delete(id: number): Promise<void> {
        return httpService.delete(`${API_BASE}/${id}`);
    } 
};